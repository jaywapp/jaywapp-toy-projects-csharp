namespace AIInstaller.Core.Models;

public sealed class ModelDefinition
{
    public required ModelIdentifier Identifier { get; init; }

    public required string DisplayName { get; init; }

    public required bool SupportsRuleSet { get; init; }

    public required IReadOnlyList<AccountConnectionMethod> SupportedConnectionMethods { get; init; }
}
