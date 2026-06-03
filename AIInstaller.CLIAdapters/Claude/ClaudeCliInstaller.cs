using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Claude;

public sealed class ClaudeCliInstaller : CliInstallerBase
{
    public ClaudeCliInstaller(ProcessCommandRunner runner, ExecutableLocator locator)
        : base(runner, locator)
    {
    }

    public override ModelIdentifier ModelIdentifier => ModelIdentifier.ClaudeCode;

    protected override string DisplayName => "Claude Code";

    protected override string CommandName => "claude";

    protected override IReadOnlyList<string> InstallCommands =>
    [
        "npm install -g @anthropic-ai/claude-code"
    ];
}
