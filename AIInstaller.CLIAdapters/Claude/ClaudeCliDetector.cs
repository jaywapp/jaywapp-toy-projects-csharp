using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Claude;

public sealed class ClaudeCliDetector : CliDetectorBase
{
    public ClaudeCliDetector(ExecutableLocator locator, ProcessCommandRunner runner)
        : base(locator, runner)
    {
    }

    public override ModelIdentifier ModelIdentifier => ModelIdentifier.ClaudeCode;

    protected override string CommandName => "claude";
}
