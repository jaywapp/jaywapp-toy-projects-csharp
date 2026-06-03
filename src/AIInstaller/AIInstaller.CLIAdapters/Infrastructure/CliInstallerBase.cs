using AIInstaller.Core.Installation;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Infrastructure;

public abstract class CliInstallerBase : ICliInstaller
{
    private readonly ProcessCommandRunner _runner;
    private readonly ExecutableLocator _locator;

    protected CliInstallerBase(ProcessCommandRunner runner, ExecutableLocator locator)
    {
        _runner = runner;
        _locator = locator;
    }

    public abstract ModelIdentifier ModelIdentifier { get; }

    protected abstract string DisplayName { get; }

    protected abstract string CommandName { get; }

    protected abstract IReadOnlyList<string> InstallCommands { get; }

    public async Task<OperationResult> InstallAsync(bool useExistingEnvironment, CancellationToken cancellationToken)
    {
        if (useExistingEnvironment)
        {
            return OperationResult.Success($"기존 {DisplayName} 환경을 그대로 사용합니다.");
        }

        OperationResult prerequisiteCheck = await ValidatePrerequisitesAsync(cancellationToken).ConfigureAwait(false);
        if (!prerequisiteCheck.IsSuccess)
        {
            return prerequisiteCheck;
        }

        ProcessCommandResult installResult = new()
        {
            IsSuccess = false,
            ExitCode = -1,
            StandardOutput = string.Empty,
            StandardError = "설치 명령이 구성되지 않았습니다."
        };

        foreach (string installCommand in InstallCommands)
        {
            installResult = await _runner
                .RunAsync("cmd.exe", $"/c {installCommand}", cancellationToken)
                .ConfigureAwait(false);

            if (installResult.IsSuccess)
            {
                break;
            }
        }

        if (!installResult.IsSuccess)
        {
            return OperationResult.Failure($"설치 실패: {ResolveErrorMessage(installResult)}");
        }

        ProcessCommandResult verifyResult = await _runner
            .RunAsync("cmd.exe", $"/c {CommandName} --version", cancellationToken)
            .ConfigureAwait(false);

        if (verifyResult.IsSuccess)
        {
            return OperationResult.Success($"설치 완료 ({verifyResult.StandardOutput}).");
        }

        string executablePath = await FindInstalledExecutableAsync(cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(executablePath))
        {
            return OperationResult.Success(
                $"설치는 완료됐지만 현재 프로세스 PATH에 아직 반영되지 않았을 수 있습니다. 확인된 실행 파일: {executablePath}");
        }

        return OperationResult.Failure(
            $"설치 명령은 성공했지만 실행 검증에 실패했습니다: {ResolveErrorMessage(verifyResult)}");
    }

    private async Task<OperationResult> ValidatePrerequisitesAsync(CancellationToken cancellationToken)
    {
        ProcessCommandResult nodeResult = await _runner
            .RunAsync("cmd.exe", "/c node --version", cancellationToken)
            .ConfigureAwait(false);
        if (!nodeResult.IsSuccess)
        {
            return OperationResult.Failure("Node.js가 설치되어 있지 않거나 PATH에 없습니다.");
        }

        ProcessCommandResult npmResult = await _runner
            .RunAsync("cmd.exe", "/c npm --version", cancellationToken)
            .ConfigureAwait(false);
        if (!npmResult.IsSuccess)
        {
            return OperationResult.Failure("npm이 설치되어 있지 않거나 PATH에 없습니다.");
        }

        return OperationResult.Success("설치 사전 점검을 통과했습니다.");
    }

    private async Task<string> FindInstalledExecutableAsync(CancellationToken cancellationToken)
    {
        string pathFromWhere = await _locator.FindPathAsync(CommandName, cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(pathFromWhere))
        {
            return pathFromWhere;
        }

        ProcessCommandResult prefixResult = await _runner
            .RunAsync("cmd.exe", "/c npm prefix -g", cancellationToken)
            .ConfigureAwait(false);
        if (!prefixResult.IsSuccess || string.IsNullOrWhiteSpace(prefixResult.StandardOutput))
        {
            return string.Empty;
        }

        string prefixPath = prefixResult.StandardOutput.Trim();
        string[] candidates =
        [
            Path.Combine(prefixPath, $"{CommandName}.cmd"),
            Path.Combine(prefixPath, $"{CommandName}.ps1"),
            Path.Combine(prefixPath, $"{CommandName}.exe")
        ];

        return candidates.FirstOrDefault(File.Exists) ?? string.Empty;
    }

    private static string ResolveErrorMessage(ProcessCommandResult result)
    {
        if (!string.IsNullOrWhiteSpace(result.StandardError))
        {
            return result.StandardError;
        }

        if (!string.IsNullOrWhiteSpace(result.StandardOutput))
        {
            return result.StandardOutput;
        }

        return $"종료 코드 {result.ExitCode}";
    }
}
