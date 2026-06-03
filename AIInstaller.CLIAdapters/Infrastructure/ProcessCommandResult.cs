namespace AIInstaller.CLIAdapters.Infrastructure;

public sealed class ProcessCommandResult
{
    public required bool IsSuccess { get; init; }

    public required int ExitCode { get; init; }

    public required string StandardOutput { get; init; }

    public required string StandardError { get; init; }
}
