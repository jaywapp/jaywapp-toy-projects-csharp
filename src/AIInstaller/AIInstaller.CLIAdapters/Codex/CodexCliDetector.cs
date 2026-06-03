using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Codex;

public sealed class CodexCliDetector : CliDetectorBase
{
    public CodexCliDetector(ExecutableLocator locator, ProcessCommandRunner runner)
        : base(locator, runner)
    {
    }

    public override ModelIdentifier ModelIdentifier => ModelIdentifier.Codex;

    protected override string CommandName => "codex";
}
