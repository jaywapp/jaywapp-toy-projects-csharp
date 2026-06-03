using AIInstaller.Core.Models;

namespace AIInstaller.Core.Services;

public sealed class ModelRegistry : IModelRegistry
{
    private static readonly IReadOnlyList<ModelDefinition> Models =
    [
        new ModelDefinition
        {
            Identifier = ModelIdentifier.Codex,
            DisplayName = "Codex",
            SupportsRuleSet = true,
            SupportedConnectionMethods =
            [
                AccountConnectionMethod.ApiKey,
                AccountConnectionMethod.CliLoginCommand
            ]
        },
        new ModelDefinition
        {
            Identifier = ModelIdentifier.ClaudeCode,
            DisplayName = "Claude Code",
            SupportsRuleSet = true,
            SupportedConnectionMethods =
            [
                AccountConnectionMethod.ApiKey,
                AccountConnectionMethod.OAuth,
                AccountConnectionMethod.CliLoginCommand
            ]
        }
    ];

    public IReadOnlyList<ModelDefinition> GetModels()
    {
        return Models;
    }
}
