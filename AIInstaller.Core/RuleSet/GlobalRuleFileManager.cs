using AIInstaller.Core.Models;

namespace AIInstaller.Core.RuleSet;

public sealed class GlobalRuleFileManager : IGlobalRuleFileManager
{
    private const string RuleFileName = "RULE.md";
    private const string ClaudeMdFileName = "CLAUDE.md";
    private const string AgentsMdFileName = "AGENTS.md";

    private const string RuleReferenceDirective =
        "이 디렉터리의 RULE.md 내용을 우선 규칙으로 읽고, 이후 작업도 그 규칙을 기준으로 진행하세요.";

    public async Task<string> LoadRuleContentAsync(string directoryPath, CancellationToken cancellationToken)
    {
        string path = Path.Combine(directoryPath, RuleFileName);
        if (File.Exists(path))
        {
            return await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
        }

        return GetDefaultTemplate();
    }

    public async Task<OperationResult> SaveAllAsync(string directoryPath, string ruleContent, CancellationToken cancellationToken)
    {
        try
        {
            Directory.CreateDirectory(directoryPath);

            string rulePath = Path.Combine(directoryPath, RuleFileName);
            await File.WriteAllTextAsync(rulePath, ruleContent, cancellationToken).ConfigureAwait(false);

            await EnsureReferenceFileAsync(
                Path.Combine(directoryPath, ClaudeMdFileName),
                "Claude Code Configuration",
                cancellationToken).ConfigureAwait(false);

            await EnsureReferenceFileAsync(
                Path.Combine(directoryPath, AgentsMdFileName),
                "Codex Agent Configuration",
                cancellationToken).ConfigureAwait(false);

            return OperationResult.Success("RULE.md, CLAUDE.md, AGENTS.md를 저장했습니다.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure($"규칙 파일 저장에 실패했습니다: {ex.Message}");
        }
    }

    private static async Task EnsureReferenceFileAsync(string filePath, string title, CancellationToken cancellationToken)
    {
        if (File.Exists(filePath))
        {
            string existing = await File.ReadAllTextAsync(filePath, cancellationToken).ConfigureAwait(false);
            if (!existing.Contains("RULE.md", StringComparison.OrdinalIgnoreCase))
            {
                string updated = $"# {title}\n\n{RuleReferenceDirective}\n\n---\n\n{existing}";
                await File.WriteAllTextAsync(filePath, updated, cancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            string content = $"# {title}\n\n{RuleReferenceDirective}\n";
            await File.WriteAllTextAsync(filePath, content, cancellationToken).ConfigureAwait(false);
        }
    }

    public static string GetDefaultTemplate()
    {
        return """
               # 공통 AI 작업 규칙

               ## 작업 범위
               - 현재 프로젝트 디렉터리와 그 하위 폴더만 수정한다.
               - 범위를 벗어나는 변경이 필요하면 먼저 이유를 기록한다.

               ## 진행 원칙
               - 막히지 않았다면 불필요한 확인 질문 없이 진행한다.
               - 실패 원인은 숨기지 말고 명확하게 남긴다.

               ## 코드 변경
               - 변경은 최소 범위로 유지한다.
               - 기존 구조와 일관성을 우선한다.

               ## 완료 기준
               - 구현 후 빌드나 검증 결과를 함께 남긴다.

               ## 응답 언어
               - 기본 응답은 한국어로 작성한다.
               """;
    }
}
