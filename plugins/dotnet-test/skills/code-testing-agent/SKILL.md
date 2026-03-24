---
name: code-testing-agent
description: >-
  Generates comprehensive, workable unit tests for any programming language.
  Use when asked to generate tests, write unit tests, improve test coverage,
  add test coverage, create test files, or test a codebase.
---

# Code Testing Agent

Generates unit tests that compile and pass, following project conventions.

## When to Use

- Generate unit tests for a project or specific files
- Improve test coverage for existing codebases
- Add tests for new or untested code

## When Not to Use

- Running existing tests → use `run-tests`
- Migrating test frameworks → use migration skills
- Writing MSTest-specific patterns → use `writing-mstest-tests`

## Workflow

### Step 1: Assess the request

- **Trivial** (single function or class, clear scope): Write the tests directly following [unit-test-generation.prompt.md](unit-test-generation.prompt.md) guidelines. Build and run to verify.
- **Non-trivial** (multiple files, whole project, coverage improvement): Proceed to Step 2.

### Step 2: Invoke the orchestrator

Call the `code-testing-generator` agent with the user's request. The orchestrator delegates each phase — research, planning, implementation — to a separate subagent call so that the heavy exploration and code-generation context stays isolated. Each subagent returns only its results (research findings, plan, implementation report) back to the orchestrator, keeping the coordination context lean. The orchestrator loops until goals are met.
