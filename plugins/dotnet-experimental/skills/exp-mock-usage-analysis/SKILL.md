---
name: exp-mock-usage-analysis
description: "Detects unused, redundant, or unnecessary mocks in .NET test suites and recommends fixes. Use when the user asks to audit mock usage, find unnecessary or unused mock setups, reduce over-mocking, identify redundant mock configurations, review whether mocks of DTOs or stable types like ILogger/IOptions are needed, or replace mocks with real implementations. Works with Moq (Mock<T>, Setup, Verify), NSubstitute (Substitute.For, Returns, Received), FakeItEasy (A.Fake, A.CallTo, MustHaveHappened), and manual test doubles."
---

# Mock Usage Analysis

Analyze .NET test code for unused, redundant, or unnecessary mocks and recommend targeted fixes — removing dead setups, merging overlapping mocks, or replacing mocks with real behavior where appropriate.

## When to Use

- User asks to audit or review mock usage in tests
- User wants to reduce over-mocking or simplify test setup
- User asks "are my mocks necessary?" or "which mocks can I remove?"
- User reports brittle tests caused by excessive mock configuration
- User wants to identify mocks that could be replaced with real implementations
- Test setup is longer than the test logic itself and the user wants to simplify

## When Not to Use

- User wants to write new mocks or set up test doubles (general testing guidance)
- User wants to detect non-mock test anti-patterns (use `test-anti-patterns`)
- User wants to refactor test structure without focusing on mocks (use `exp-test-maintainability`)
- User wants to migrate between mock frameworks (out of scope)

## Inputs

| Input | Required | Description |
|-------|----------|-------------|
| Test code | Yes | One or more test files or classes to analyze |
| Production code | Yes | The code under test — needed to determine which dependencies are trivial or stable |
| Runtime data | No | Output from mock verification tools, coverage reports, or custom profiler traces showing which mock setups were actually invoked at runtime |
| Specific concern | No | A focused area like "unused setups" or "trivial mocks" to narrow the review |

## Workflow

### Step 1: Gather test and production code

Read the test files the user wants reviewed. **Always** read the corresponding production code — this is essential for determining whether a mocked dependency is trivial, stable, or has complex behavior worth isolating.

Identify all mock-related code by scanning for:

| Framework | Setup patterns | Verification patterns |
|-----------|---------------|----------------------|
| **Moq** | `new Mock<T>()`, `mock.Setup(...)`, `Mock.Of<T>(...)`, `mock.SetupGet(...)`, `mock.SetupSet(...)`, `mock.SetupSequence(...)` | `mock.Verify(...)`, `mock.VerifyAll()`, `mock.VerifyNoOtherCalls()` |
| **NSubstitute** | `Substitute.For<T>()`, `sub.Method(...).Returns(...)`, `sub.When(...).Do(...)` | `sub.Received(...)`, `sub.DidNotReceive(...)`, `sub.ReceivedWithAnyArgs(...)` |
| **FakeItEasy** | `A.Fake<T>()`, `A.CallTo(() => fake.Method(...)).Returns(...)`, `A.CallTo(...).DoesNothing()` | `A.CallTo(...).MustHaveHappened()`, `A.CallTo(...).MustNotHaveHappened()` |
| **Manual** | Classes implementing interfaces with hardcoded returns, `Stub*` or `Fake*` prefixed classes | Direct field inspection on fake instances |

### Step 2: Classify each mock

For every mocked dependency, determine its category:

#### 2a: Trace mock usage through the test

For each mock setup, trace whether its return value or side effect is actually consumed during the test execution path:

- **Used**: The mock's setup is exercised by the Act step and its result influences the assertion or the code path.
- **Unused**: The mock is set up but never invoked during the test, or its return value is discarded. Check for setups that exist "just in case" or were left over from refactoring.
- **Partially used**: A mock has multiple setups but only some are invoked. Common with `SetupSequence` or multiple `Setup` calls on the same mock.

#### 2b: Assess the mocked dependency

Examine the production type being mocked:

| Category | Characteristics | Typical recommendation |
|----------|----------------|----------------------|
| **Trivial / data object** | DTOs, POCOs, record types, enums, value objects with no behavior. No side effects, no I/O, deterministic. | **Remove mock** — use a real instance. `new CustomerDto { Name = "Alice" }` is clearer than `mock.Setup(x => x.Name).Returns("Alice")`. |
| **Stable utility** | Pure functions, simple calculators, formatters, validators with no dependencies. Deterministic, fast, no I/O. | **Remove mock** — use the real implementation. Mocking `Math.Round` or `StringFormatter.Format` adds complexity without value. |
| **Thin wrapper** | Simple delegating types that pass through to another dependency (e.g., a repository that just calls DbContext). | **Consider removing** — if the wrapper is simple enough, test through it. If the underlying dependency is mockable, mock that instead. |
| **External boundary** | HTTP clients, databases, file systems, message queues, third-party APIs, system clocks. I/O-bound, non-deterministic, or slow. | **Keep mock** — these are valid isolation boundaries. |
| **Complex collaborator** | Types with significant business logic, state machines, workflow orchestrators. | **Keep mock if needed for isolation** — but be aware this couples tests to implementation details. Consider testing through the real collaborator if feasible. |

### Step 3: Detect specific anti-patterns

Scan for these mock-specific problems:

#### Critical — Mocks that hide bugs or give false confidence

| Anti-Pattern | What to Look For | Fix |
|---|---|---|
| **Mock mirrors implementation** | Mock setup replicates the exact production logic (e.g., `mock.Setup(x => x.Calculate(2, 3)).Returns(5)` when the real method does `a + b`). The test verifies the mock, not the code. | Remove the mock; use the real implementation and assert the result. |
| **Untested mock setup** | Mock is set up with `.Returns(...)` but the test never exercises the code path that calls it. Setup exists to avoid null references, not to test behavior. | Remove the unused setup. If NullReferenceException occurs, that's a real bug to investigate. |
| **Mock verifies only interaction** | Test has `mock.Verify(x => x.Save(...), Times.Once())` but no assertion on the actual output or state change. Test passes even if the business logic is wrong as long as Save is called. | Add behavioral assertions on the result. Use verify only as a secondary check. |

#### High — Mocks that hurt maintainability

| Anti-Pattern | What to Look For | Fix |
|---|---|---|
| **Mocking owned types** | Mocking interfaces/classes defined in the same project being tested. This usually indicates the test is coupled to implementation details. | Test through the real type or restructure the dependency. |
| **Setup sprawl** | 10+ mock setup lines for a single test. The mock configuration becomes the dominant part of the test, obscuring what's actually being tested. | Extract a builder/factory, merge setups into a reusable fixture, or question whether so many dependencies indicate the SUT does too much. |
| **Redundant mock per test** | Multiple tests create identical mock configurations independently. | Extract shared mock setup to a helper method or test fixture. |
| **Mocking concrete classes** | `new Mock<ConcreteService>()` requires the class to be unsealed with virtual methods, leaks implementation, and is fragile. | Mock the interface instead, or use the real class. |

#### Medium — Unnecessary mocking complexity

| Anti-Pattern | What to Look For | Fix |
|---|---|---|
| **Mocking trivial types** | Mocking DTOs, POCOs, records, enums, or value objects that have no behavior. `mock.Setup(x => x.Name).Returns("Test")` when `new Dto { Name = "Test" }` works. | Replace with real instance construction. |
| **Mocking stable utilities** | Mocking `ILogger<T>`, `IOptions<T>`, simple validators, or pure functions where the real behavior is deterministic and fast. | Use `NullLogger<T>.Instance`, `Options.Create(new MyOptions { ... })`, or the real implementation. |
| **Over-configured mock** | Mock setup includes `.Callback(...)`, `.Verifiable()`, `.SetupAllProperties()` when simpler setup suffices. | Simplify to the minimum setup needed. |
| **Mocking `IEnumerable<T>` or collections** | Creating a mock for `IList<T>` or `IEnumerable<T>` instead of passing `new List<T> { ... }`. | Use real collections. |

#### Low — Style and convention issues

| Anti-Pattern | What to Look For | Fix |
|---|---|---|
| **Inconsistent mock style** | Mix of `new Mock<T>()` and `Mock.Of<T>()` in same test class without reason. Or mixing strict/loose mock behavior. | Pick one style and be consistent. |
| **Mock variable naming** | Generic names like `mock1`, `mock2` instead of descriptive names like `mockRepository`, `mockLogger`. | Use descriptive names. |
| **Unused mock.Object** | `mock.Object` assigned to a variable but never passed to the SUT. | Remove the mock entirely. |

### Step 4: Incorporate runtime information (if available)

If the user provides runtime data (profiler output, coverage reports, mock verification logs):

1. **Cross-reference runtime traces** with static analysis findings. A setup that looks used in code may never be reached at runtime due to conditional logic.
2. **Identify hot mock paths** — setups invoked many times across multiple tests are good candidates for shared fixtures.
3. **Detect true dead code** — setups never invoked across the entire test suite run, not just a single test.
4. **Validate removal safety** — if runtime data confirms a setup is never hit, removal is safe. If no runtime data exists, flag the removal as "likely safe, verify by running tests."

### Step 5: Generate recommendations

For each finding, produce an actionable recommendation:

1. **Remove** — Delete the mock setup, replace with real instance or nothing.
2. **Merge** — Combine redundant mock configurations into shared helpers or fixtures.
3. **Replace** — Swap mock with real implementation, `NullLogger`, `Options.Create`, or a simpler test double.
4. **Simplify** — Reduce mock configuration to the minimum needed (remove unnecessary `.Callback`, `.Verifiable`, etc.).
5. **Keep** — Explicitly note mocks that are correctly used as isolation boundaries.

### Step 6: Report findings

Present findings in this structure:

1. **Summary** — Mock count, issues found by severity, estimated lines removable. If mock usage is already well-calibrated, lead with that assessment.
2. **Critical and High findings** — Each with location, explanation, and before/after code fix.
3. **Medium and Low findings** — Summarize in a table unless the user wants detail.
4. **Positive observations** — Call out well-placed mocks (external boundaries, proper isolation). Don't only report negatives.
5. **Aggregate recommendations** — If many tests share the same problem (e.g., all mock `ILogger`), provide a single fix strategy rather than listing every instance.

## Calibration Rules

Apply these before reporting:

- **Don't recommend removing mocks for external boundaries.** HttpClient, databases, file systems, message queues, and third-party APIs are valid isolation targets.
- **Respect strict mock policies.** If the codebase uses `MockBehavior.Strict` deliberately, don't suggest switching to Loose without understanding the reason.
- **Context matters for `ILogger` mocking.** If tests verify specific log messages were emitted (e.g., for audit trails), `ILogger` mocking is valid. Only flag it when the mock is set up but log output is never asserted.
- **Builders are not always better.** Don't recommend a `MockBuilder` pattern for 2-3 setup lines. Only suggest extraction at 5+ setups repeated across 3+ tests.
- **Production code complexity determines mock validity.** A mock for a type with 1 method returning a constant is wasteful. A mock for a type with 5 methods, state, and I/O is reasonable.
- **If mocks are well-chosen, say so clearly.** A review finding zero Critical/High issues is a valid outcome. Don't inflate findings.

## Validation

- [ ] Every finding includes a specific location (file, method, line)
- [ ] Every Critical/High finding includes before/after code fix
- [ ] Production code was reviewed to assess mock necessity (not just test code)
- [ ] Trivial/stable types are correctly identified (checked the real implementation)
- [ ] Recommendations distinguish between "remove" (safe), "likely removable" (needs verification), and "keep"
- [ ] Positive observations are included for well-placed mocks

## Common Pitfalls

| Pitfall | Solution |
|---------|----------|
| Recommending real implementations for I/O types | Keep mocks for HTTP, DB, filesystem — these are valid isolation boundaries |
| Flagging `ILogger` mocking without checking assertions | Some tests verify log output for audit compliance — check before flagging |
| Suggesting mock removal without reading production code | Always read the real type to assess complexity before recommending removal |
| Recommending builders for simple setups | Only suggest shared fixtures when 5+ lines repeat in 3+ tests |
| Treating all `Verify` calls as anti-patterns | Interaction verification is valid for fire-and-forget operations (e.g., sending events). Only flag when verify replaces behavioral assertions |
| Ignoring framework idioms | `Mock.Of<T>()` (Moq), `Substitute.For<T>()` (NSubstitute), and `A.Fake<T>()` (FakeItEasy) each have different best practices — use correct terminology |
| Flagging manual test doubles as problems | Hand-written stubs/fakes are often simpler and more readable than framework mocks — acknowledge when they're appropriate |
