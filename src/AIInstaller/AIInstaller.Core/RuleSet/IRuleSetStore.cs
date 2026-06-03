using AIInstaller.Core.Models;

namespace AIInstaller.Core.RuleSet;

public interface IRuleSetStore
{
    Task<RuleSetDocument?> LoadAsync(string fileName, CancellationToken cancellationToken);

    Task SaveAsync(string fileName, RuleSetDocument document, CancellationToken cancellationToken);

    Task<IReadOnlyList<ModelRuleSetMapping>> LoadMappingAsync(CancellationToken cancellationToken);

    Task SaveMappingAsync(IReadOnlyList<ModelRuleSetMapping> mapping, CancellationToken cancellationToken);
}
