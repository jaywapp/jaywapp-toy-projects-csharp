# AIInstaller 점검 보고서

작성일: 2026-03-21

## 요약

코드를 전반적으로 확인한 결과, 현재 프로젝트는 "빌드는 되지만 사용 흐름은 쉽게 실패하거나 오동작할 수 있는 상태"에 가깝다. 가장 큰 문제는 다음 5가지다.

1. 설치/저장 실패 여부와 무관하게 마법사 단계가 계속 진행된다.
2. 문서와 설계에서 강조한 계정 연결 흐름이 실제 앱에서는 거의 구현되지 않았다.
3. CLI 연결 상태 판정이 실제 로그인 검증이 아니라 "설정 파일 존재 여부"에만 의존한다.
4. 한글 문자열과 템플릿이 광범위하게 깨져 있어 UI와 생성 파일 품질이 크게 떨어진다.
5. 저장소 루트의 `nul` 파일이 Windows 도구 체인을 실제로 깨뜨리고 있다.

## 주요 발견 사항

### 1. 실패해도 다음 단계로 넘어감

심각도: 높음

근거:
- `AIInstaller.App/ViewModels/MainViewModel.cs:249-256`
- `AIInstaller.App/ViewModels/MainViewModel.cs:258-265`
- `AIInstaller.App/ViewModels/MainViewModel.cs:309-338`
- `AIInstaller.App/ViewModels/MainViewModel.cs:361-396`

설명:
- Step 2에서 `InstallRequiredAsync()` 실행 후 성공 여부를 확인하지 않고 바로 Step 3으로 이동한다.
- Step 3에서 `SaveRulesAsync()` 실행 후 성공 여부를 확인하지 않고 바로 Step 4로 이동한다.
- 두 메서드는 내부에서 예외를 잡고 `StatusMessage`만 바꾸기 때문에, 상위 호출자는 실패를 알 수 없다.

영향:
- 설치 실패 후에도 사용자는 "다음 단계"로 넘어가므로 앱이 정상 동작한 것처럼 보인다.
- 규칙 파일 저장 실패 후에도 완료 화면으로 이동해 실제 결과와 UI 상태가 어긋난다.

권장 수정:
- `InstallRequiredAsync`, `SaveRulesAsync`, `RefreshAsync`가 `OperationResult`를 반환하도록 바꾸고, 실패 시 `CurrentStep`을 진행시키지 않도록 수정한다.

### 2. 계정 연결 기능이 실질적으로 빠져 있음

심각도: 높음

근거:
- `AIInstaller.App/Services/InstallerDashboardService.cs:28-43`
- `AIInstaller.CLIAdapters/Infrastructure/AccountConnectorBase.cs:7-49`
- `AIInstaller.Core/Installation/IAccountConnector.cs:5-11`

설명:
- `ClaudeAccountConnector`, `CodexAccountConnector`, `IAccountConnector` 구현은 존재한다.
- 하지만 `InstallerDashboardService`에서는 detector와 installer만 등록하고 account connector는 전혀 생성하거나 사용하지 않는다.
- UI에도 "연결하기" 동작이 없다.

영향:
- 프로젝트 설명과 위키는 설치 후 계정 연결까지 다루는 것처럼 보이지만, 실제 앱은 연결 기능을 제공하지 않는다.
- `Installed` 상태에 머문 사용자가 앱 안에서 문제를 해결할 수 없다.

권장 수정:
- 서비스 계층에 connector 레지스트리를 추가하고, Step 2 또는 별도 Step에서 `ConnectAsync()`를 실행할 수 있게 해야 한다.

### 3. 연결 상태 판정이 지나치게 약함

심각도: 높음

근거:
- `AIInstaller.CLIAdapters/Infrastructure/CliDetectorBase.cs:59-71`
- `AIInstaller.CLIAdapters/Infrastructure/AccountConnectorBase.cs:17-25`

설명:
- 현재 연결 여부는 계정 설정 파일 존재 여부만으로 판정된다.
- 파일이 남아 있기만 해도 `InstalledAndConnected`로 처리된다.
- 반대로 토큰 만료, 잘못된 프로필, 깨진 설정, 다른 계정 로그인 등은 전혀 검증하지 않는다.

영향:
- 앱 상태 표가 실제 인증 상태와 다를 가능성이 높다.
- "설치는 됐는데 실제 CLI 호출은 안 되는" 상황을 정상으로 오판할 수 있다.

권장 수정:
- 최소한 CLI별 인증 확인 명령을 별도로 정의하고, 파일 존재 여부는 보조 지표로만 사용해야 한다.

### 4. 한글 문자열/템플릿 인코딩이 광범위하게 깨져 있음

심각도: 높음

근거:
- `AIInstaller.App/MainWindow.xaml:27`
- `AIInstaller.App/MainWindow.xaml:89`
- `AIInstaller.App/ViewModels/MainViewModel.cs:38-41`
- `AIInstaller.App/Services/InstallerDashboardService.cs:97`
- `AIInstaller.Core/RuleSet/GlobalRuleFileManager.cs:11-12`
- `README.md`

설명:
- UI 텍스트, 상태 메시지, 기본 RULE 템플릿, README까지 한글이 깨져 있다.
- `GlobalRuleFileManager.GetDefaultTemplate()`는 생성 파일 내용 자체가 손상된 상태다.

영향:
- UI 신뢰도가 크게 떨어진다.
- 생성되는 `RULE.md`, `CLAUDE.md`, `AGENTS.md` 내용이 깨져 사용 가치가 낮다.
- 문서와 실제 동작이 모두 읽기 어렵다.

권장 수정:
- 모든 소스/문서를 UTF-8로 통일하고, 깨진 문자열을 원문 기준으로 복구해야 한다.
- 문자열 리소스는 별도 리소스 파일로 분리하는 편이 안전하다.

### 5. 저장소 루트의 `nul` 파일이 Windows 도구를 깨뜨림

심각도: 높음

근거:
- 저장소 루트 `nul`
- `git status --short`에서 `?? nul`
- `rg` 실행 시 `rg: ./nul: 잘못된 기능입니다. (os error 1)`

설명:
- Windows 예약명인 `nul`이 루트에 존재한다.
- 실제로 `rg` 검색이 이 파일 때문에 에러를 내고 있다.
- PowerShell에서도 일반 파일처럼 다루기 어렵다.

영향:
- 개발 도구, 검색, 스크립트, CI 일부가 불안정해질 수 있다.
- 문제 분석과 유지보수 효율이 크게 떨어진다.

권장 수정:
- 이 파일이 의도된 것이 아니라면 즉시 삭제하고 재발 방지 규칙을 추가한다.

### 6. 규칙 파일 기본 저장 경로가 사용자 홈으로 떨어질 수 있음

심각도: 중간

근거:
- `AIInstaller.App/Services/InstallerDashboardService.cs:46-61`

설명:
- 앱 기준 디렉터리부터 상위로 올라가며 `CLAUDE.md`, `AGENTS.md`, `RULE.md`를 찾고, 없으면 사용자 홈 디렉터리를 반환한다.
- 사용자가 저장 경로를 인지하지 못한 채 진행하면 홈 디렉터리에 규칙 파일이 생길 수 있다.

영향:
- 엉뚱한 위치에 설정 파일이 생성될 수 있다.
- 프로젝트별 규칙 관리라는 기대와 실제 저장 위치가 어긋난다.

권장 수정:
- 현재 작업 중인 프로젝트 경로를 명시적으로 선택하게 하거나, 기본 경로를 앱 실행 폴더가 아닌 사용자가 지정한 폴더로 제한해야 한다.

### 7. RuleSet 저장소를 만들지만 실제 동작에는 거의 쓰지 않음

심각도: 중간

근거:
- `AIInstaller.App/Services/InstallerDashboardService.cs:66`
- `AIInstaller.App/Services/InstallerDashboardService.cs:145-189`
- `AIInstaller.Core/RuleSet/JsonRuleSetStore.cs:5-53`

설명:
- 앱 시작 시 기본 ruleset, custom ruleset, mapping 파일을 AppData에 만든다.
- 하지만 실제 UI는 `RULE.md` 직접 편집만 제공하며, ruleset과 mapping은 이후 로직에서 소비되지 않는다.

영향:
- 유지보수 포인트만 늘고 기능 가치는 거의 없다.
- 설계와 구현이 분리돼 있다.

권장 수정:
- ruleset 기반 기능을 실제 UI/생성 로직에 연결하거나, 아니면 현재 단계에서는 제거해 구조를 단순화해야 한다.

### 8. 패키지 호환성 경고를 안고 빌드 중

심각도: 중간

근거:
- `AIInstaller.App/AIInstaller.App.csproj:8-9`
- `dotnet build AIInstaller.sln` 결과 `NU1701` 경고 4건

설명:
- `Prism.Core 6.3.0`, `ReactiveUI.WPF 19.4.1`이 `net8.0-windows`에 맞는 패키지로 복원되지 않았다.
- 실제 빌드는 통과했지만, 런타임 호환성 위험이 남아 있다.

영향:
- 런타임 예외나 예기치 않은 바인딩 문제 가능성이 있다.

권장 수정:
- `net8.0-windows`와 명시적으로 호환되는 버전으로 교체하거나, 의존성 자체를 재검토해야 한다.

### 9. 설치 명령과 검증 흐름이 단순해서 실패 원인 분리가 어려움

심각도: 중간

근거:
- `AIInstaller.CLIAdapters/Infrastructure/CliInstallerBase.cs:38-63`
- `AIInstaller.CLIAdapters/Codex/CodexCliInstaller.cs:19-23`
- `AIInstaller.CLIAdapters/Claude/ClaudeCliInstaller.cs:19-22`

설명:
- 설치는 `cmd /c npm install -g ...`만 순차 실행하고, 검증은 `--version` 성공 여부만 본다.
- Node/npm 부재, PATH 갱신 지연, 권한 문제, 네트워크 문제, 패키지 명령 실패를 구분해서 안내하지 않는다.
- Codex는 두 개의 패키지를 후보로 두는데 왜 두 개인지 설명이나 우선순위 정책이 없다.

영향:
- 사용자는 "설치 실패"만 보고 실제 원인을 알기 어렵다.
- 유지보수 시에도 어떤 패키지가 기준인지 모호하다.

권장 수정:
- Node/npm 사전 점검, PATH 재조회, 오류 유형별 메시지, 설치 대상 패키지 정책 정리가 필요하다.

### 10. UI/주석/문서의 단계 표현이 서로 어긋남

심각도: 낮음

근거:
- `AIInstaller.App/MainWindow.xaml:313`
- `AIInstaller.App/MainWindow.xaml:336`
- `README.md`

설명:
- DataGrid 주석은 `Steps 1,2,3,5`로 되어 있고, Rule Editor 주석은 `Step 4`로 되어 있지만 실제 로직은 Step 3에서 Rule Editor를 보여준다.
- README는 계정 연결을 포함한 흐름처럼 읽히는데 UI는 4단계이며 계정 연결 단계가 없다.

영향:
- 코드를 읽는 사람과 사용자 모두 혼란스럽다.

권장 수정:
- UI 단계 정의와 문서를 현재 구현과 일치시키거나, 구현을 문서에 맞추어 완성해야 한다.

## 빠른 우선순위 제안

1. `nul` 파일 제거
2. 인코딩 복구
3. Step 진행 조건을 성공 기반으로 변경
4. 계정 연결 기능 실제 연결
5. 연결 상태 검증 로직 강화
6. 패키지 버전/설치 정책 정리

## 확인한 내용

- `dotnet build AIInstaller.sln` 실행 결과: 빌드 성공, `NU1701` 경고 4건
- 자동 테스트 프로젝트는 보이지 않았고 별도 테스트는 실행하지 못했다
- GUI 앱 직접 조작 재현은 하지 않았고, 코드 경로와 빌드 결과 중심으로 점검했다
