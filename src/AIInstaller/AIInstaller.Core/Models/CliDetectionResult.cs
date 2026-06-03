namespace AIInstaller.Core.Models;

public sealed class CliDetectionResult
{
    public required ModelIdentifier ModelIdentifier { get; init; }

    public required CliInstallationStatus Status { get; init; }

    public required string ExecutablePath { get; init; }

    public required string Version { get; init; }

    public required string Diagnostics { get; init; }
}
