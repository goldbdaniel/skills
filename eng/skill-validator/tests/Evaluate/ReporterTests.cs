using SkillValidator.Evaluate;

namespace SkillValidator.Tests;

public class ReporterTests
{
    private static SkillVerdict PassingVerdict() => new()
    {
        SkillName = "test-skill",
        SkillPath = "plugins/test/skills/test-skill",
        Passed = true,
        Scenarios = [],
        OverallImprovementScore = 0.5,
        Reason = "Passed",
    };

    [Fact]
    public void Footer_WithoutCommit_OmitsCommitEntry()
    {
        var md = Reporter.GenerateMarkdownSummary([PassingVerdict()], "m", "j");

        Assert.DoesNotContain("Commit:", md);
        Assert.Contains("Model: m | Judge: j", md);
    }

    [Fact]
    public void Footer_WithCommitShaOnly_RendersBactickShortSha()
    {
        var sha = "abc1234567890";
        var md = Reporter.GenerateMarkdownSummary([PassingVerdict()], "m", "j", commitSha: sha);

        Assert.Contains("Commit: `abc1234`", md);
        Assert.DoesNotContain("](", md.Split('\n').Last(l => l.Contains("Commit:")));
    }

    [Fact]
    public void Footer_WithCommitShaAndUrl_RendersHyperlink()
    {
        var sha = "abc1234567890";
        var url = "https://github.com/dotnet/skills/commit/abc1234567890";
        var md = Reporter.GenerateMarkdownSummary([PassingVerdict()], "m", "j", commitSha: sha, commitUrl: url);

        Assert.Contains($"Commit: [abc1234]({url})", md);
    }

    [Fact]
    public void Footer_ShortShaUnder7Chars_UsesFullSha()
    {
        var sha = "abc12";
        var md = Reporter.GenerateMarkdownSummary([PassingVerdict()], "m", "j", commitSha: sha);

        Assert.Contains("Commit: `abc12`", md);
    }

    [Fact]
    public void Footer_EmptyCommitSha_OmitsCommitEntry()
    {
        var md = Reporter.GenerateMarkdownSummary([PassingVerdict()], "m", "j", commitSha: "");

        Assert.DoesNotContain("Commit:", md);
    }

    [Fact]
    public void Footer_WhitespaceCommitSha_OmitsCommitEntry()
    {
        var md = Reporter.GenerateMarkdownSummary([PassingVerdict()], "m", "j", commitSha: "   ");

        Assert.DoesNotContain("Commit:", md);
    }

    [Fact]
    public void Footer_CommitShaWithWhitespace_TrimsBeforeShortening()
    {
        var sha = "  abc1234567890  ";
        var md = Reporter.GenerateMarkdownSummary([PassingVerdict()], "m", "j", commitSha: sha);

        Assert.Contains("Commit: `abc1234`", md);
    }
}
