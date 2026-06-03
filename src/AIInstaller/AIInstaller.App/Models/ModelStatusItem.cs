using AIInstaller.Core.Models;

namespace AIInstaller.App.Models;

public sealed class ModelStatusItem
{
    public required ModelIdentifier ModelIdentifier { get; init; }

    public required CliInstallationStatus RawStatus { get; init; }

    public required string DisplayName { get; init; }

    public required string Status { get; init; }

    public required string RecommendedAction { get; init; }

    public required string Version { get; init; }

    public required string ExecutablePath { get; init; }

    public required string Diagnostics { get; init; }
}
