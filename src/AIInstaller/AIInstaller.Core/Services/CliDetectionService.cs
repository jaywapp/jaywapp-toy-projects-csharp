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
                try
                {
                    CliDetectionResult result = await detector.DetectAsync(cancellationToken).ConfigureAwait(false);
                    results.Add(result);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Trace.TraceWarning("CLI detection failed for {0} ({1}).",
                        model.Identifier, exception.GetType().Name);
                    results.Add(new CliDetectionResult
                    {
                        ModelIdentifier = model.Identifier,
                        Status = CliInstallationStatus.InvalidOrBroken,
                        ExecutablePath = string.Empty,
                        Version = string.Empty,
                        Diagnostics = $"Detection failed ({exception.GetType().Name})."
                    });
                }
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
