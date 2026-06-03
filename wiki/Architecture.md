# Architecture

## 구성

AIInstaller는 세 개의 프로젝트로 나뉩니다.

```text
AIInstaller.App          WPF UI, ViewModel, 대시보드 서비스
AIInstaller.CLIAdapters  CLI별 감지/설치/로그인 연결 구현
AIInstaller.Core         공통 모델, 인터페이스, 규칙 파일 관리
```

의존성 방향은 `App -> CLIAdapters -> Core` 입니다.

## 책임 분리

### AIInstaller.Core

- `Detection`: `ICliDetector`
- `Installation`: `ICliInstaller`, `IAccountConnector`
- `Models`: 상태와 결과 모델
- `RuleSet`: 규칙 파일 저장/생성 인터페이스
- `Services`: 모델 레지스트리와 감지 서비스

### AIInstaller.CLIAdapters

- `Infrastructure`: 프로세스 실행, 실행 파일 탐색, 공통 베이스 클래스
- `Claude`: Claude Code 전용 detector / installer / connector
- `Codex`: Codex 전용 detector / installer / connector

### AIInstaller.App

- `ViewModels`: 마법사 상태 관리
- `Services`: App 계층 오케스트레이션
- `Models`: UI 표시 모델
- `MainWindow.xaml`: 메인 마법사 UI

## 현재 동작 방식

1. `InstallerDashboardService`가 모든 detector를 호출해 CLI 상태를 수집합니다.
2. 설치가 필요한 항목은 installer로 처리합니다.
3. 로그인 정보가 부족한 항목은 account connector로 확인하거나 로그인 창을 엽니다.
4. 규칙 저장 단계에서 `RULE.md`, `CLAUDE.md`, `AGENTS.md`를 맞춰 저장합니다.

## MVVM

- ViewModel 기반 상태 관리
- Prism `DelegateCommand` 사용
- WPF Data Binding 사용

## NuGet 패키지

- `Prism.Core` 9.0.537
- `WPF-UI` 4.2.0
