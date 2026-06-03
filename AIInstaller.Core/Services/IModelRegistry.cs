using AIInstaller.Core.Models;

namespace AIInstaller.Core.Services;

public interface IModelRegistry
{
    IReadOnlyList<ModelDefinition> GetModels();
}
