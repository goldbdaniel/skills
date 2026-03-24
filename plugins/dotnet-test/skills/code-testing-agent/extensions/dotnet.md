# .NET Extension

## Commands

| Scope | Command |
|-------|---------|
| Build test project | `dotnet build MyProject.Tests.csproj` |
| Build solution (final) | `dotnet build MySolution.sln --no-incremental` |
| Run tests | `dotnet test MyProject.Tests.csproj` |
| Lint | `dotnet format --include path/to/file.cs` |

Use `--no-incremental` for final validation — incremental builds hide errors like CS7036.

## Project References

Before writing test code, verify the test `.csproj` has `<ProjectReference>` entries for each source project. Missing references cause CS0234/CS0246.

```xml
<ProjectReference Include="../SourceProject/SourceProject.csproj" />
```

## Common Errors

| Error | Fix |
|-------|-----|
| CS0234/CS0246 | Add `<ProjectReference>` or `using` statement |
| CS1061 | Verify method/property name matches source exactly |
| CS0029 | Fix type mismatch — cast or change type |
| CS7036 | Read constructor/method signature, pass all required args |

## Multi-targeting

When a source project targets multiple frameworks (e.g., `<TargetFrameworks>net8.0;net9.0</TargetFrameworks>`), the test project should target a single compatible framework. Don't multi-target the test project unless explicitly required.

## MSTest Template

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectName.Tests;

[TestClass]
public sealed class ClassNameTests
{
    [TestMethod]
    [DataRow(2, 3, 5, DisplayName = "Positive numbers")]
    [DataRow(-1, 1, 0, DisplayName = "Negative and positive")]
    public void MethodName_Scenario_Expected(int a, int b, int expected)
    {
        var sut = new ClassName();
        var result = sut.MethodName(a, b);
        Assert.AreEqual(expected, result);
    }
}
```

## Coverage XML Parsing

If `.testagent/initial_coverage.xml` exists, it uses Cobertura/VS format:

- `module` elements with `line_coverage` attribute — identifies which assemblies have low coverage
- `function` elements with `line_coverage="0.00"` — identifies completely untested methods
- `range` elements with `covered="no"` — identifies specific uncovered lines

## Skip Coverage Tools

Do not configure or run code coverage measurement tools (coverlet, dotnet-coverage, XPlat Code Coverage). These tools have inconsistent cross-configuration behavior and waste significant time. Coverage is measured separately by the evaluation harness.
