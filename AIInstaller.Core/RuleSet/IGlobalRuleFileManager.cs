using AIInstaller.Core.Models;

namespace AIInstaller.Core.RuleSet;

public interface IGlobalRuleFileManager
{
    Task<string> LoadRuleContentAsync(string directoryPath, CancellationToken cancellationToken);

    Task<OperationResult> SaveAllAsync(string directoryPath, string ruleContent, CancellationToken cancellationToken);
}
