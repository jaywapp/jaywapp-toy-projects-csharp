using AIInstaller.Core.Detection;
using AIInstaller.Core.Models;

namespace AIInstaller.Core.Services;

public sealed class CliDetectionService
{
    private readonly IReadOnlyDictionary<ModelIdentifier, ICliDetector> _detectors;

    public CliDetectionService(IEnumerable<ICliDetector> detectors)
    {
        _detectors = detectors.ToDictionary(detector => detector.ModelIdentifier);
    }

    public async Task<IReadOnlyList<CliDetectionResult>> DetectAllAsync(
        IReadOnlyList<ModelDefinition> models,
        CancellationToken cancellationToken)
    {
        List<CliDetectionResult> results = new(models.Count);

        foreach (ModelDefinition model in models)
        {
            if (_detectors.TryGetValue(model.Identifier, out ICliDetector? detector))
            {
                CliDetectionResult result = await detector.DetectAsync(cancellationToken).ConfigureAwait(false);
                results.Add(result);
            }
            else
            {
                results.Add(new CliDetectionResult
                {
                    ModelIdentifier = model.Identifier,
                    Status = CliInstallationStatus.NotInstalled,
                    ExecutablePath = string.Empty,
                    Version = string.Empty,
                    Diagnostics = "Detector is not registered."
                });
            }
        }

        return results;
    }
}
