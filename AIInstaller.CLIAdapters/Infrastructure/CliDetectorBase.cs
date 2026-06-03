using AIInstaller.Core.Detection;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Infrastructure;

public abstract class CliDetectorBase : ICliDetector
{
    private readonly ExecutableLocator _locator;
    private readonly ProcessCommandRunner _runner;

    protected CliDetectorBase(ExecutableLocator locator, ProcessCommandRunner runner)
    {
        _locator = locator;
        _runner = runner;
    }

    public abstract ModelIdentifier ModelIdentifier { get; }

    protected abstract string CommandName { get; }

    public async Task<CliDetectionResult> DetectAsync(CancellationToken cancellationToken)
    {
        ProcessCommandResult versionResult = await _runner
            .RunAsync("cmd.exe", $"/c {CommandName} --version", cancellationToken)
            .ConfigureAwait(false);

        string executablePath = await _locator
            .FindPathAsync(CommandName, cancellationToken)
            .ConfigureAwait(false);

        if (!versionResult.IsSuccess && string.IsNullOrWhiteSpace(executablePath))
        {
            return new CliDetectionResult
            {
                ModelIdentifier = ModelIdentifier,
                Status = CliInstallationStatus.NotInstalled,
                ExecutablePath = string.Empty,
                Version = string.Empty,
                Diagnostics = $"{CommandName} 명령을 PATH에서 찾지 못했습니다."
            };
        }

        if (!versionResult.IsSuccess)
        {
            return new CliDetectionResult
            {
                ModelIdentifier = ModelIdentifier,
                Status = CliInstallationStatus.InvalidOrBroken,
                ExecutablePath = executablePath,
                Version = string.Empty,
                Diagnostics = string.IsNullOrWhiteSpace(versionResult.StandardError)
                    ? $"{CommandName} 실행 파일은 찾았지만 버전 확인에 실패했습니다."
                    : $"{CommandName} 버전 확인 실패: {versionResult.StandardError}"
            };
        }

        string resolvedPath = string.IsNullOrWhiteSpace(executablePath) ? "PATH 해석 성공" : executablePath;
        string versionText = string.IsNullOrWhiteSpace(versionResult.StandardOutput)
            ? versionResult.StandardError
            : versionResult.StandardOutput;

        return new CliDetectionResult
        {
            ModelIdentifier = ModelIdentifier,
            Status = CliInstallationStatus.Installed,
            ExecutablePath = resolvedPath,
            Version = versionText,
            Diagnostics = "CLI 실행 확인 완료"
        };
    }
}
