using AIInstaller.Core.Models;

namespace AIInstaller.Core.Services;

public interface IExecutionValidator
{
    Task<OperationResult> ValidateAsync(string executableName, string arguments, CancellationToken cancellationToken);
}
