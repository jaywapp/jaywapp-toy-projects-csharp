using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Claude;

public sealed class ClaudeAccountConnector : AccountConnectorBase
{
    public override ModelIdentifier ModelIdentifier => ModelIdentifier.ClaudeCode;

    protected override string DisplayName => "Claude Code";

    protected override string LoginCommand => "claude";

    protected override IReadOnlyList<string> EvidenceFilePaths =>
    [
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude.json"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude", "settings.json")
    ];

    protected override IReadOnlyList<string> EvidenceEnvironmentVariables =>
    [
        "ANTHROPIC_API_KEY"
    ];

    protected override string LoginInstructions =>
        "열린 Claude Code 창에서 /login을 실행하거나 API 키를 설정한 뒤 창을 닫고 다시 확인하세요.";
}
