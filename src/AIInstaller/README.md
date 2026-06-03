# AIInstaller

Windows에서 Claude Code와 Codex CLI를 점검, 설치, 로그인 연결, 규칙 파일 생성까지 한 번에 진행하는 WPF 앱입니다.

## 현재 지원 범위

- Claude Code
- Codex

## 마법사 흐름

1. 설치 상태 확인
2. 설치 및 로그인 연결
3. `RULE.md` 편집과 `CLAUDE.md` / `AGENTS.md` 참조 파일 저장
4. 최종 상태 확인

## 주요 기능

- CLI 설치 여부, 실행 가능 여부, 버전 확인
- 설치되지 않았거나 손상된 CLI 재설치
- 로그인 정보 확인 및 필요 시 로그인 창 실행
- 프로젝트 폴더에 `RULE.md`, `CLAUDE.md`, `AGENTS.md` 생성

## 기술 스택

- .NET 8
- WPF
- Prism.Core
- WPF-UI

## 프로젝트 구조

```text
AIInstaller.App/          WPF UI, ViewModel, Dashboard Service
AIInstaller.CLIAdapters/  Claude/Codex별 감지, 설치, 로그인 연결 구현
AIInstaller.Core/         공통 모델, 인터페이스, 규칙 파일 관리
Docs/                     점검 문서와 보조 문서
wiki/                     아키텍처/문제 해결 문서
```

## 빌드

```powershell
dotnet build AIInstaller.sln
```

## 실행

```powershell
dotnet run --project AIInstaller.App
```

## 규칙 파일 동작

Step 3에서 저장 경로를 지정하고 내용을 저장하면 다음 파일을 맞춰 생성합니다.

- `RULE.md`
- `CLAUDE.md`
- `AGENTS.md`

`CLAUDE.md`, `AGENTS.md`에는 같은 폴더의 `RULE.md`를 먼저 읽도록 안내 문구가 들어갑니다.

## CLI 추가 방법

1. `AIInstaller.Core/Models/ModelIdentifier.cs`에 모델 식별자 추가
2. `AIInstaller.Core/Services/ModelRegistry.cs`에 모델 등록
3. `AIInstaller.CLIAdapters/`에 detector, installer, account connector 구현 추가

## 라이선스

MIT
