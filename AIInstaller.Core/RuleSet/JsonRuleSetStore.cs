using System.Text.Json;
using AIInstaller.Core.Models;

namespace AIInstaller.Core.RuleSet;

public sealed class JsonRuleSetStore : IRuleSetStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _ruleSetRootPath;

    public JsonRuleSetStore(string ruleSetRootPath)
    {
        _ruleSetRootPath = ruleSetRootPath;
        Directory.CreateDirectory(_ruleSetRootPath);
    }

    public async Task<RuleSetDocument?> LoadAsync(string fileName, CancellationToken cancellationToken)
    {
        string path = Path.Combine(_ruleSetRootPath, fileName);
        if (!File.Exists(path))
        {
            return null;
        }

        await using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<RuleSetDocument>(stream, SerializerOptions, cancellationToken).ConfigureAwait(false);
    }

    public async Task SaveAsync(string fileName, RuleSetDocument document, CancellationToken cancellationToken)
    {
        string path = Path.Combine(_ruleSetRootPath, fileName);
        await using FileStream stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, document, SerializerOptions, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<ModelRuleSetMapping>> LoadMappingAsync(CancellationToken cancellationToken)
    {
        string path = Path.Combine(_ruleSetRootPath, "model-mapping.json");
        if (!File.Exists(path))
        {
            return Array.Empty<ModelRuleSetMapping>();
        }

        await using FileStream stream = File.OpenRead(path);
        IReadOnlyList<ModelRuleSetMapping>? mapping = await JsonSerializer
            .DeserializeAsync<IReadOnlyList<ModelRuleSetMapping>>(stream, SerializerOptions, cancellationToken)
            .ConfigureAwait(false);

        return mapping ?? Array.Empty<ModelRuleSetMapping>();
    }

    public async Task SaveMappingAsync(IReadOnlyList<ModelRuleSetMapping> mapping, CancellationToken cancellationToken)
    {
        string path = Path.Combine(_ruleSetRootPath, "model-mapping.json");
        await using FileStream stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, mapping, SerializerOptions, cancellationToken).ConfigureAwait(false);
    }
}
