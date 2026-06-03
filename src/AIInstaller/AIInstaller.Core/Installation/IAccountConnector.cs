using AIInstaller.Core.Models;

namespace AIInstaller.Core.Installation;

public interface IAccountConnector
{
    ModelIdentifier ModelIdentifier { get; }

    Task<OperationResult> ValidateConnectionAsync(CancellationToken cancellationToken);

    Task<OperationResult> ConnectAsync(CancellationToken cancellationToken);
}
