using System.CommandLine;
using System.Text.RegularExpressions;

namespace SkillValidator.Evaluate;

public static class ConsolidateCommand
{
    public static Command Create()
    {
        var filesArg = new Argument<string[]>("files") { Description = "Paths to results.json files to merge" };
        var outputOpt = new Option<string>("--output") { Description = "Output file path for the consolidated markdown", Required = true };
        var commitOpt = new Option<string?>("--commit") { Description = "Commit SHA to include in the output (used to link to the PR commit)" };

        var command = new Command("consolidate", "Consolidate multiple results.json files into a single markdown summary")
        {
            filesArg,
            outputOpt,
            commitOpt,
        };

        command.SetAction(async (parseResult, _) =>
        {
            var files = parseResult.GetValue(filesArg) ?? [];
            var output = parseResult.GetValue(outputOpt)!;
            var commit = parseResult.GetValue(commitOpt);
            return await Consolidate(files, output, commit);
        });

        return command;
    }

    private static async Task<int> Consolidate(string[] files, string outputPath, string? commitSha)
    {
        if (files.Length == 0)
        {
            await File.WriteAllTextAsync(outputPath, "## Skill Validation Results\n\nNo results were produced.\n");
            Console.WriteLine($"No input files provided. Wrote fallback to {outputPath}");
            return 0;
        }

        var allVerdicts = new List<SkillVerdict>();
        string? model = null;
        string? judgeModel = null;

        foreach (var file in files)
        {
            try
            {
                var content = await File.ReadAllTextAsync(file);
                var data = System.Text.Json.JsonSerializer.Deserialize(content,
                    SkillValidatorJsonContext.Default.ConsolidateData);
                if (data?.Verdicts is not null)
                    allVerdicts.AddRange(data.Verdicts);
                if (data?.Model is not null && model is null) model = data.Model;
                if (data?.JudgeModel is not null && judgeModel is null) judgeModel = data.JudgeModel;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Failed to parse {file}: {error}");
            }
        }

        string? commitUrl = null;
        if (commitSha is not null)
        {
            // Validate that the SHA contains only valid hex characters before embedding in a URL
            if (!Regex.IsMatch(commitSha, @"^[0-9a-f]+$", RegexOptions.IgnoreCase))
            {
                Console.Error.WriteLine($"Warning: --commit value '{commitSha}' contains non-hex characters; ignoring.");
                commitSha = null;
            }
            else
            {
                var serverUrl = Environment.GetEnvironmentVariable("GITHUB_SERVER_URL");
                var repo = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
                if (!string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(repo))
                    commitUrl = $"{serverUrl.TrimEnd('/')}/{repo}/commit/{commitSha}";
            }
        }

        var output = Reporter.GenerateMarkdownSummary(allVerdicts, model, judgeModel, commitSha, commitUrl);
        await File.WriteAllTextAsync(outputPath, output);
        Console.WriteLine($"Consolidated {files.Length} result file(s) into {outputPath}");
        return 0;
    }
}
