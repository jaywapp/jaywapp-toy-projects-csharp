using System.Diagnostics;

namespace AIInstaller.CLIAdapters.Infrastructure;

public sealed class ProcessCommandRunner
{
    public async Task<ProcessCommandResult> RunAsync(string fileName, string arguments, CancellationToken cancellationToken)
    {
        try
        {
            using Process process = new();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            Task<string> stdoutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            Task<string> stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            await Task.WhenAll(stdoutTask, stderrTask).ConfigureAwait(false);
            string stdout = stdoutTask.Result;
            string stderr = stderrTask.Result;

            return new ProcessCommandResult
            {
                IsSuccess = process.ExitCode == 0,
                ExitCode = process.ExitCode,
                StandardOutput = stdout.Trim(),
                StandardError = stderr.Trim()
            };
        }
        catch (Exception ex)
        {
            return new ProcessCommandResult
            {
                IsSuccess = false,
                ExitCode = -1,
                StandardOutput = string.Empty,
                StandardError = ex.Message
            };
        }
    }
}
