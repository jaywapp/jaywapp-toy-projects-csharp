using System.Diagnostics;
using AIInstaller.Core.Installation;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Infrastructure;

public abstract class AccountConnectorBase : IAccountConnector
{
    public abstract ModelIdentifier ModelIdentifier { get; }

    protected abstract string DisplayName { get; }

    protected abstract string LoginCommand { get; }

    protected abstract IReadOnlyList<string> EvidenceFilePaths { get; }

    protected virtual IReadOnlyList<string> EvidenceEnvironmentVariables => Array.Empty<string>();

    protected virtual string LoginInstructions => "로그인을 완료한 뒤 창을 닫고 다시 확인하세요.";

    public Task<OperationResult> ValidateConnectionAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (string variableName in EvidenceEnvironmentVariables)
        {
            string? value = Environment.GetEnvironmentVariable(variableName);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return Task.FromResult(OperationResult.Success($"{DisplayName} 인증 환경 변수를 확인했습니다."));
            }
        }

        foreach (string path in EvidenceFilePaths)
        {
            if (!File.Exists(path))
            {
                continue;
            }

            FileInfo fileInfo = new(path);
            if (fileInfo.Length > 0)
            {
                return Task.FromResult(OperationResult.Success($"{DisplayName} 계정 설정 파일을 확인했습니다."));
            }
        }

        return Task.FromResult(OperationResult.Failure(
            $"{DisplayName} 로그인 정보를 찾지 못했습니다. {LoginInstructions}"));
    }

    public async Task<OperationResult> ConnectAsync(CancellationToken cancellationToken)
    {
        OperationResult preValidation = await ValidateConnectionAsync(cancellationToken).ConfigureAwait(false);
        if (preValidation.IsSuccess)
        {
            return preValidation;
        }

        try
        {
            using Process process = new();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k {LoginCommand}",
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

            process.Start();
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

            OperationResult validation = await ValidateConnectionAsync(cancellationToken).ConfigureAwait(false);
            return validation.IsSuccess
                ? OperationResult.Success($"{DisplayName} 로그인 연결을 확인했습니다.")
                : OperationResult.Failure(validation.Message);
        }
        catch (Exception ex)
        {
            return OperationResult.Failure($"{DisplayName} 로그인 연결에 실패했습니다: {ex.Message}");
        }
    }
}
