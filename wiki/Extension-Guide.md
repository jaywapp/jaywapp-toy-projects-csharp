# 새 CLI 추가 가이드

## 개요

AIInstaller는 어댑터 패턴을 사용하므로, 새로운 AI CLI 도구를 최소한의 코드로 추가할 수 있습니다.

## 추가 절차

### 1. ModelIdentifier 추가

`AIInstaller.Core/Models/ModelIdentifier.cs`에 열거형 값을 추가합니다.

```csharp
public enum ModelIdentifier
{
    Codex,
    ClaudeCode,
    NewCli        // 추가
}
```

### 2. ModelRegistry에 등록

`AIInstaller.Core/Services/ModelRegistry.cs`에 모델 정의를 추가합니다.

```csharp
new ModelDefinition
{
    Identifier = ModelIdentifier.NewCli,
    DisplayName = "New CLI",
    InstallationMethods = [InstallationMethod.Npm],
    AccountConnectionMethods = [AccountConnectionMethod.ApiKey],
    SupportsRuleSets = true
}
```

### 3. Detector 구현

`AIInstaller.CLIAdapters/NewCli/NewCliDetector.cs`를 생성합니다.

```csharp
using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.NewCli;

public sealed class NewCliDetector : CliDetectorBase
{
    public NewCliDetector(ExecutableLocator locator, ProcessCommandRunner runner)
        : base(locator, runner) { }

    public override ModelIdentifier ModelIdentifier => ModelIdentifier.NewCli;

    protected override string CommandName => "newcli";

    protected override string AccountConfigPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".newcli",
            "config.json");
}
```

`CliDetectorBase`가 자동으로 처리하는 것:
- `newcli --version` 실행하여 설치 여부 확인
- `where newcli`로 실행 경로 탐색
- `~/.newcli/config.json` 존재 여부로 계정 연결 상태 확인

### 4. Installer 구현

`AIInstaller.CLIAdapters/NewCli/NewCliInstaller.cs`를 생성합니다.

```csharp
using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.NewCli;

public sealed class NewCliInstaller : CliInstallerBase
{
    public NewCliInstaller(ProcessCommandRunner runner) : base(runner) { }

    public override ModelIdentifier ModelIdentifier => ModelIdentifier.NewCli;

    protected override string DisplayName => "New CLI";

    protected override string CommandName => "newcli";

    protected override IReadOnlyList<string> InstallCommands =>
        ["npm install -g @vendor/newcli"];
}
```

`CliInstallerBase`가 자동으로 처리하는 것:
- `InstallCommands` 목록을 순서대로 시도
- 첫 번째 성공 시 중단
- 설치 후 `newcli --version`으로 검증

### 5. InstallerDashboardService에 등록

`AIInstaller.App/Services/InstallerDashboardService.cs`의 생성자에 등록합니다.

```csharp
IReadOnlyList<ICliDetector> detectors =
[
    new CodexCliDetector(locator, runner),
    new ClaudeCliDetector(locator, runner),
    new NewCliDetector(locator, runner)          // 추가
];

IReadOnlyList<ICliInstaller> installers =
[
    new CodexCliInstaller(runner),
    new ClaudeCliInstaller(runner),
    new NewCliInstaller(runner)                  // 추가
];
```

### 6. 빌드 및 확인

```bash
dotnet build AIInstaller.sln
dotnet run --project AIInstaller.App
```

Step 1에서 새 CLI의 상태가 DataGrid에 자동으로 표시됩니다.

## 베이스 클래스 요약

| 베이스 클래스 | 오버라이드 필수 항목 | 자동 처리 |
|---|---|---|
| `CliDetectorBase` | `ModelIdentifier`, `CommandName`, `AccountConfigPath` | 버전 확인, 경로 탐색, 설정 파일 확인 |
| `CliInstallerBase` | `ModelIdentifier`, `DisplayName`, `CommandName`, `InstallCommands` | 명령 순차 실행, 폴백, 설치 후 검증 |
| `AccountConnectorBase` | `ModelIdentifier`, `DisplayName`, `AccountConfigPath`, `LoginCommand` | 대화형 프로세스 실행, 연결 검증 |

## 프로젝트 파일 구조 예시

```
AIInstaller.CLIAdapters/
├── Infrastructure/
│   ├── CliDetectorBase.cs
│   ├── CliInstallerBase.cs
│   └── AccountConnectorBase.cs
├── Claude/
│   ├── ClaudeCliDetector.cs
│   └── ClaudeCliInstaller.cs
├── Codex/
│   ├── CodexCliDetector.cs
│   └── CodexCliInstaller.cs
└── NewCli/                          ← 새로 추가
    ├── NewCliDetector.cs
    └── NewCliInstaller.cs
```
