using AIInstaller.Core.Detection;
using AIInstaller.Core.Models;
using AIInstaller.Core.Services;

var models = new[] { Model(ModelIdentifier.Codex), Model(ModelIdentifier.ClaudeCode) };
var service = new CliDetectionService(new[] { new FailingDetector() });
Check.That((await service.DetectAllAsync(Array.Empty<ModelDefinition>(), CancellationToken.None)).Count == 0, "Empty models");
var results = await service.DetectAllAsync(models, CancellationToken.None);
Check.That(results[0].Status == CliInstallationStatus.InvalidOrBroken, "Detector exception becomes broken status");
Check.That(results[0].Diagnostics.Contains(nameof(UnauthorizedAccessException)), "Traceable error category");
Check.That(!results[0].Diagnostics.Contains("sensitive-fixture"), "Exception text is not exposed");
Check.That(results[1].Status == CliInstallationStatus.NotInstalled, "Other models still detected");
using var cancellation = new CancellationTokenSource(); cancellation.Cancel();
await Check.ThrowsAsync<OperationCanceledException>(() => new CliDetectionService(new[] { new CancelDetector() }).DetectAllAsync(models, cancellation.Token), "Cancellation propagates");
var validator = new ProcessExecutionValidator();
var invalid = await validator.ValidateAsync("missing-regression-executable-7654321", "", CancellationToken.None);
Check.That(!invalid.IsSuccess, "Missing executable returns failure");
Console.WriteLine($"PASS {Check.Count} installer regression checks");

static ModelDefinition Model(ModelIdentifier id) => new() { Identifier = id, DisplayName = id.ToString(), SupportsRuleSet = false, SupportedConnectionMethods = [] };

sealed class FailingDetector : ICliDetector
{
    public ModelIdentifier ModelIdentifier => ModelIdentifier.Codex;
    public Task<CliDetectionResult> DetectAsync(CancellationToken cancellationToken) => throw new UnauthorizedAccessException("sensitive-fixture");
}

sealed class CancelDetector : ICliDetector
{
    public ModelIdentifier ModelIdentifier => ModelIdentifier.Codex;
    public Task<CliDetectionResult> DetectAsync(CancellationToken cancellationToken) => Task.FromCanceled<CliDetectionResult>(cancellationToken);
}
