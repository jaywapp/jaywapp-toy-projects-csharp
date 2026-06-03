using AIInstaller.CLIAdapters.Infrastructure;
using AIInstaller.Core.Models;

namespace AIInstaller.CLIAdapters.Codex;

public sealed class CodexAccountConnector : AccountConnectorBase
{
    public override ModelIdentifier ModelIdentifier => ModelIdentifier.Codex;

    protected override string DisplayName => "Codex";

    protected override string LoginCommand => "codex --login";

    protected override IReadOnlyList<string> EvidenceFilePaths =>
    [
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".codex", "auth.json"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".codex", "config.toml")
    ];

    protected override IReadOnlyList<string> EvidenceEnvironmentVariables =>
    [
        "OPENAI_API_KEY"
    ];

    protected override string LoginInstructions =>
        "브라우저 로그인 또는 API 키 설정을 완료한 뒤 창을 닫고 다시 확인하세요.";
}
