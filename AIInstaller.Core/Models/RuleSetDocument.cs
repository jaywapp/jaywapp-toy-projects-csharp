namespace AIInstaller.Core.Models;

public sealed class RuleSetDocument
{
    public required string Name { get; init; }

    public required string Version { get; init; }

    public required IReadOnlyList<string> Rules { get; init; }
}
