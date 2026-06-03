using System.IO;
using AIInstaller.App.Models;
using AIInstaller.CLIAdapters.Claude;
using AIInstaller.CLIAdapters.Codex;
using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Detection;
using AIInstaller.Core.Installation;
using AIInstaller.Core.Models;
using AIInstaller.Core.RuleSet;
using AIInstaller.Core.Services;

namespace AIInstaller.App.Services;

public sealed class InstallerDashboardService
{
    private readonly IModelRegistry _modelRegistry;
    private readonly CliDetectionService _detectionService;
    private readonly IGlobalRuleFileManager _globalRuleFileManager;
    private readonly IReadOnlyDictionary<ModelIdentifier, ICliInstaller> _installers;
    private readonly IReadOnlyDictionary<ModelIdentifier, IAccountConnector> _connectors;

    public InstallerDashboardService()
    {
        _modelRegistry = new ModelRegistry();

        ProcessCommandRunner runner = new();
        ExecutableLocator locator = new(runner);
        IReadOnlyList<ICliDetector> detectors =
        [
            new CodexCliDetector(locator, runner),
            new ClaudeCliDetector(locator, runner)
        ];

        IReadOnlyList<ICliInstaller> installers =
        [
            new CodexCliInstaller(runner, locator),
            new ClaudeCliInstaller(runner, locator)
        ];

        IReadOnlyList<IAccountConnector> connectors =
        [
            new CodexAccountConnector(),
            new ClaudeAccountConnector()
        ];

        _detectionService = new CliDetectionService(detectors);
        _installers = installers.ToDictionary(installer => installer.ModelIdentifier);
        _connectors = connectors.ToDictionary(connector => connector.ModelIdentifier);
        _globalRuleFileManager = new GlobalRuleFileManager();
    }

    public string GetDefaultRuleDirectoryPath()
    {
        DirectoryInfo? dir = new(AppDomain.CurrentDomain.BaseDirectory);
        while (dir != null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, ".git")) ||
                File.Exists(Path.Combine(dir.FullName, "AIInstaller.sln")) ||
                File.Exists(Path.Combine(dir.FullName, "CLAUDE.md")) ||
                File.Exists(Path.Combine(dir.FullName, "AGENTS.md")) ||
                File.Exists(Path.Combine(dir.FullName, "RULE.md")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public async Task<IReadOnlyList<ModelStatusItem>> LoadModelStatusesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ModelDefinition> models = _modelRegistry.GetModels();
        IReadOnlyList<CliDetectionResult> results = await _detectionService.DetectAllAsync(models, cancellationToken).ConfigureAwait(false);

        List<ModelStatusItem> items = new(results.Count);
        foreach (CliDetectionResult result in results)
        {
            ModelDefinition definition = models.First(model => model.Identifier == result.ModelIdentifier);
            CliDetectionResult enrichedResult = await EnrichConnectionStatusAsync(result, cancellationToken).ConfigureAwait(false);

            items.Add(new ModelStatusItem
            {
                ModelIdentifier = enrichedResult.ModelIdentifier,
                RawStatus = enrichedResult.Status,
                DisplayName = definition.DisplayName,
                Status = ToDisplayStatus(enrichedResult.Status),
                RecommendedAction = ToRecommendedAction(enrichedResult.Status),
                Version = string.IsNullOrWhiteSpace(enrichedResult.Version) ? "-" : enrichedResult.Version,
                ExecutablePath = string.IsNullOrWhiteSpace(enrichedResult.ExecutablePath) ? "-" : enrichedResult.ExecutablePath,
                Diagnostics = string.IsNullOrWhiteSpace(enrichedResult.Diagnostics) ? "-" : enrichedResult.Diagnostics
            });
        }

        return items;
    }

    public async Task<OperationResult> InstallRequiredAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ModelStatusItem> currentStatuses = await LoadModelStatusesAsync(cancellationToken).ConfigureAwait(false);
        IReadOnlyList<ModelStatusItem> installTargets = currentStatuses
            .Where(item => item.RawStatus == CliInstallationStatus.NotInstalled || item.RawStatus == CliInstallationStatus.InvalidOrBroken)
            .ToList();

        if (installTargets.Count == 0)
        {
            return OperationResult.Success("추가 설치가 필요한 CLI가 없습니다.");
        }

        List<string> messages = new(installTargets.Count);
        bool allSucceeded = true;

        foreach (ModelStatusItem target in installTargets)
        {
            if (!_installers.TryGetValue(target.ModelIdentifier, out ICliInstaller? installer))
            {
                allSucceeded = false;
                messages.Add($"{target.DisplayName}: 설치 구현이 등록되지 않았습니다.");
                continue;
            }

            OperationResult result = await installer.InstallAsync(useExistingEnvironment: false, cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                allSucceeded = false;
            }

            messages.Add($"{target.DisplayName}: {result.Message}");
        }

        string summary = string.Join(Environment.NewLine, messages);
        return allSucceeded ? OperationResult.Success(summary) : OperationResult.Failure(summary);
    }

    public async Task<OperationResult> ConnectRequiredAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ModelStatusItem> currentStatuses = await LoadModelStatusesAsync(cancellationToken).ConfigureAwait(false);
        IReadOnlyList<ModelStatusItem> connectTargets = currentStatuses
            .Where(item => item.RawStatus == CliInstallationStatus.Installed)
            .ToList();

        if (connectTargets.Count == 0)
        {
            return OperationResult.Success("추가 로그인 연결이 필요한 CLI가 없습니다.");
        }

        List<string> messages = new(connectTargets.Count);
        bool allSucceeded = true;

        foreach (ModelStatusItem target in connectTargets)
        {
            if (!_connectors.TryGetValue(target.ModelIdentifier, out IAccountConnector? connector))
            {
                allSucceeded = false;
                messages.Add($"{target.DisplayName}: 로그인 연결 구현이 등록되지 않았습니다.");
                continue;
            }

            OperationResult result = await connector.ConnectAsync(cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                allSucceeded = false;
            }

            messages.Add($"{target.DisplayName}: {result.Message}");
        }

        string summary = string.Join(Environment.NewLine, messages);
        return allSucceeded ? OperationResult.Success(summary) : OperationResult.Failure(summary);
    }

    public async Task<string> LoadRuleContentAsync(string directoryPath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            throw new InvalidOperationException("규칙 파일을 저장할 디렉터리를 선택하세요.");
        }

        return await _globalRuleFileManager.LoadRuleContentAsync(directoryPath, cancellationToken).ConfigureAwait(false);
    }

    public async Task<OperationResult> SaveRulesAsync(string directoryPath, string ruleContent, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            return OperationResult.Failure("규칙 파일을 저장할 디렉터리를 입력하세요.");
        }

        return await _globalRuleFileManager.SaveAllAsync(directoryPath, ruleContent, cancellationToken).ConfigureAwait(false);
    }

    private async Task<CliDetectionResult> EnrichConnectionStatusAsync(CliDetectionResult result, CancellationToken cancellationToken)
    {
        if (result.Status != CliInstallationStatus.Installed)
        {
            return result;
        }

        if (!_connectors.TryGetValue(result.ModelIdentifier, out IAccountConnector? connector))
        {
            return result;
        }

        OperationResult connectionResult = await connector.ValidateConnectionAsync(cancellationToken).ConfigureAwait(false);
        if (connectionResult.IsSuccess)
        {
            return new CliDetectionResult
            {
                ModelIdentifier = result.ModelIdentifier,
                Status = CliInstallationStatus.InstalledAndConnected,
                ExecutablePath = result.ExecutablePath,
                Version = result.Version,
                Diagnostics = connectionResult.Message
            };
        }

        return new CliDetectionResult
        {
            ModelIdentifier = result.ModelIdentifier,
            Status = CliInstallationStatus.Installed,
            ExecutablePath = result.ExecutablePath,
            Version = result.Version,
            Diagnostics = connectionResult.Message
        };
    }

    private static string ToDisplayStatus(CliInstallationStatus status)
    {
        return status switch
        {
            CliInstallationStatus.NotInstalled => "미설치",
            CliInstallationStatus.Installed => "설치됨, 로그인 필요",
            CliInstallationStatus.InstalledAndConnected => "설치 및 연결 완료",
            CliInstallationStatus.InvalidOrBroken => "설치 손상 또는 실행 실패",
            _ => "확인 필요"
        };
    }

    private static string ToRecommendedAction(CliInstallationStatus status)
    {
        return status switch
        {
            CliInstallationStatus.NotInstalled => "설치 필요",
            CliInstallationStatus.Installed => "로그인 연결 필요",
            CliInstallationStatus.InstalledAndConnected => "완료",
            CliInstallationStatus.InvalidOrBroken => "재설치 필요",
            _ => "상태 점검"
        };
    }
}
