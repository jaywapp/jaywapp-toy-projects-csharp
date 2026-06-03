using System.Diagnostics;
using AIInstaller.Core.Models;

namespace AIInstaller.Core.Services;

public sealed class ProcessExecutionValidator : IExecutionValidator
{
    public async Task<OperationResult> ValidateAsync(string executableName, string arguments, CancellationToken cancellationToken)
    {
        try
        {
            using Process process = new();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = executableName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            Task<string> outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            Task<string> errorTask = process.StandardError.ReadToEndAsync(cancellationToken);
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            await Task.WhenAll(outputTask, errorTask).ConfigureAwait(false);
            string output = outputTask.Result;
            string error = errorTask.Result;

            if (process.ExitCode == 0)
            {
                return OperationResult.Success(string.IsNullOrWhiteSpace(output) ? "Execution succeeded." : output.Trim());
            }

            return OperationResult.Failure(string.IsNullOrWhiteSpace(error) ? "Execution failed." : error.Trim());
        }
        catch (Exception ex)
        {
            return OperationResult.Failure($"Execution exception: {ex.Message}");
        }
    }
}
