---
description: >-
  Core guidelines for generating unit tests with high coverage
  across any programming language
---

# Unit Test Generation Guidelines

## Discover Conventions First

Before writing tests, check the codebase for:

- Where test files live and how they're named
- Which testing, mocking, and assertion frameworks are used
- Existing test patterns, base classes, or utilities

Follow discovered conventions. If none exist, use best judgment.

## Core Principles

| Principle | Description |
|-----------|-------------|
| **Minimal but comprehensive** | Avoid redundant tests; aim for 80%+ coverage |
| **Parameterize** | Use `[DataRow]`, `[Theory]`, `@pytest.mark.parametrize` instead of duplicate methods |
| **Arrange-Act-Assert** | Structure every test: setup, action, verification |
| **Unit tests over integration** | Mock external dependencies; never call URLs, bind ports, or depend on timing |
| **Fix assertions, not code** | When tests fail, fix the expected value to match actual production behavior |
| **Never skip** | Don't use `[Ignore]`, `[Skip]`, or `[Inconclusive]` to pass |
| **Naming** | `Method_Condition_ExpectedResult` pattern |

## Coverage Types

- **Happy path**: valid inputs produce expected outputs
- **Edge cases**: empty, null, boundary, zero, negative, special characters
- **Error cases**: invalid inputs, exceptions, timeouts

## Before Writing Tests

1. Read source code line by line; verify exact method signatures (parameter types, count, order)
2. Identify dependencies that need mocking
3. Verify the test project references all needed source projects
4. Check `extensions/` for language-specific guidance
