using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Codex;

public sealed class CodexCliInstaller : CliInstallerBase
{
    public CodexCliInstaller(ProcessCommandRunner runner, ExecutableLocator locator)
        : base(runner, locator)
    {
    }

    public override ModelIdentifier ModelIdentifier => ModelIdentifier.Codex;

    protected override string DisplayName => "Codex";

    protected override string CommandName => "codex";

    protected override IReadOnlyList<string> InstallCommands =>
    [
        "npm install -g @openai/codex"
    ];
}
