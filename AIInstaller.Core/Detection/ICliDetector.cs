using AIInstaller.Core.Models;

namespace AIInstaller.Core.Detection;

public interface ICliDetector
{
    ModelIdentifier ModelIdentifier { get; }

    Task<CliDetectionResult> DetectAsync(CancellationToken cancellationToken);
}
