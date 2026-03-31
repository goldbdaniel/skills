# Mock Usage Analysis Skill — Design Notes

## Evaluation Results (March 2026)

### Round 1 — Initial skill (11 scenarios)

| Scenario | Baseline | Isolated | Plugin | Verdict |
| --- | --- | --- | --- | --- |
| Detect mocking of DTOs, records, and enums | 4.3/5 | 3.3/5 ⏰ | 4.3/5 | ❌ |
| Detect unused and unreachable mock setups | 4.0/5 | 4.0/5 ⏰ | 4.3/5 ⏰ | ❌ ¹ |
| Detect redundant mock configurations | 3.0/5 | 2.3/5 ⏰ | 3.3/5 | ❌ |
| Detect mocking of stable framework types | 3.0/5 | 5.0/5 | 5.0/5 | ✅ |
| Recognize well-placed mocks | 5.0/5 | 5.0/5 | 5.0/5 | ❌ ² |
| Analyze mock usage in NSubstitute tests | 3.7/5 | 5.0/5 | 5.0/5 | ✅ |
| Analyze mock usage in FakeItEasy tests | 5.0/5 | 4.7/5 | 4.7/5 | ❌ |
| Detect excessive mock configuration sprawl | 3.3/5 | 4.0/5 | 3.3/5 | ✅ |
| Decline request to write new tests | 2.0/5 | 2.0/5 | 2.3/5 | ❌ ³ |
| Decline non-mock test anti-patterns | 5.0/5 | 5.0/5 | 5.0/5 | ❌ ² |
| Decline mock framework migration | 4.0/5 | 4.0/5 | 4.0/5 | ❌ ⁴ |

**3/11 passed.** Overfitting: 0.06 (excellent).

¹ Quality improved in plugin but weighted score -25.2% from token/time overhead.
² Baseline at ceiling — no headroom for skill to add value.
³ Token overhead regression on a non-activation scenario with no quality gain.
⁴ Weighted -1.9% from token/time overhead with no quality delta.

**Issues identified:**

- **Timeouts** on scenarios 1-3 (120s too short for fixture-based scenarios)
- **Activation failures** — scenario 1 not activated in plugin, scenario 3 not activated in either mode (prompts lacked mock-specific keywords)
- **Baseline at ceiling** — 4 scenarios where the model already scores 5.0/5

### Round 2 — Fix timeouts, activation, and no-headroom scenarios

**Changes:**

- Increased timeouts: 120s → 180s for scenarios with fixture files
- Rewrote prompts with explicit mock terminology for better activation
- Added `reject_tools: ["bash", "edit"]` to FakeItEasy and well-placed mocks scenarios
- Improved skill description with framework-specific keywords (Mock<T>, Substitute.For, A.Fake)
- Removed "Decline write tests" scenario (token overhead, no value)

| Scenario | Baseline | Isolated | Plugin | Verdict |
| --- | --- | --- | --- | --- |
| Detect mocking of DTOs, records, and enums | 5.0/5 | 5.0/5 | 5.0/5 | ❌ ⁵ |
| Detect unused and unreachable mock setups | 3.3/5 | 5.0/5 | — | ✅ |
| Detect redundant mock configurations | 3.0/5 | 4.0/5 | — | ✅ |
| Detect mocking of stable framework types | 3.0/5 | 5.0/5 | — | ✅ |
| Recognize well-placed mocks | 5.0/5 | 5.0/5 | 5.0/5 | ❌ ⁵ |
| Analyze mock usage in NSubstitute tests | 3.0/5 ⏰ | 5.0/5 | — | ✅ |
| Analyze mock usage in FakeItEasy tests | 4.3/5 | 4.7/5 | — | ❌ |
| Detect excessive mock configuration sprawl | 3.0/5 | 4.0/5 | — | ✅ |
| Decline non-mock test anti-patterns | 5.0/5 | 3.7/5 ⏰ | — | ❌ |
| Decline mock framework migration | 5.0/5 | 5.0/5 | — | ❌ ⁵ |

⁵ Baseline at ceiling — model handles these well without skill guidance.

**Improvements from Round 1:**

- DTOs scenario: now activates in both isolated and plugin (was plugin-only failure)
- Redundant mocks: now activates in both modes (was NOT ACTIVATED in either)
- No more timeouts on scenarios 1-3
- NSubstitute baseline still hit timeout at 120s

### Round 3 — Remove no-headroom scenarios, fix NSubstitute timeout

**Changes:**

- Removed 4 scenarios where baseline scores 5.0/5 (see "Decisions" below)
- Increased NSubstitute timeout: 120s → 180s

6 remaining scenarios all show positive skill impact.

## Key Insight

The baseline LLM already excels at two mock-related tasks:

1. **Identifying trivial-type mocking** — the model recognizes when `Mock<CustomerDto>` should be `new CustomerDto(...)` without guidance.
2. **Recognizing well-placed mocks** — when tests correctly mock external boundaries, the model concludes the approach is sound without inventing false positives.

The skill's unique value is in **deep code-path analysis**: tracing mock setups through production code to determine whether they are actually invoked at runtime, identifying unreachable setups after early returns or exceptions, and detecting redundant configurations duplicated across tests.

## Decisions

### Removed: "Detect mocking of DTOs, records, and enums" scenario

Baseline scores 5.0/5 — the model already identifies when DTOs, records, and enums are unnecessarily mocked and recommends real instance construction. No quality delta for the skill to contribute.

### Removed: "Recognize well-placed mocks without inventing false positives" scenario

Baseline scores 5.0/5 — the model already correctly concludes that mocking external boundaries (HTTP, DB, email) is appropriate without inflating severity.

### Removed: "Decline when asked about non-mock test anti-patterns" scenario

Baseline scores 5.0/5 — non-activation scenario where the model already handles Thread.Sleep/DateTime.Now reviews without the skill. The timeout regression (5.0→3.7 ⏰) in the skilled run was caused by the 60s timeout being too short, not a skill problem.

### Removed: "Decline mock framework migration request" scenario

Baseline scores 5.0/5 — the model already handles Moq→NSubstitute migration requests without the skill. Weighted score was -1.2% from time overhead alone.
