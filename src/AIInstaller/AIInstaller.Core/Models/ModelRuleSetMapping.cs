namespace AIInstaller.Core.Models;

public sealed class ModelRuleSetMapping
{
    public required ModelIdentifier ModelIdentifier { get; init; }

    public required string RuleSetFileName { get; init; }
}
