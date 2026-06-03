using AIInstaller.CLIAdapters.Infrastructure;

namespace AIInstaller.CLIAdapters.Infrastructure;

public sealed class ExecutableLocator
{
    private readonly ProcessCommandRunner _runner;

    public ExecutableLocator(ProcessCommandRunner runner)
    {
        _runner = runner;
    }

    public async Task<string> FindPathAsync(string commandName, CancellationToken cancellationToken)
    {
        ProcessCommandResult result = await _runner.RunAsync("cmd.exe", $"/c where {commandName}", cancellationToken).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            return string.Empty;
        }

        string? firstLine = result.StandardOutput
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault();

        return firstLine ?? string.Empty;
    }
}
