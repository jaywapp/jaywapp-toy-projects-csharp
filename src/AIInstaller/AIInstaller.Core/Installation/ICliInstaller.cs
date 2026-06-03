using AIInstaller.Core.Models;

namespace AIInstaller.Core.Installation;

public interface ICliInstaller
{
    ModelIdentifier ModelIdentifier { get; }

    Task<OperationResult> InstallAsync(bool useExistingEnvironment, CancellationToken cancellationToken);
}
