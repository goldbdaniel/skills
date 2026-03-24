# Repository Instructions

This repository contains skill plugins under `plugins/`. Release plugins live under `plugins/release/<plugin-name>/` (stable, domain-specific skills) and experimental plugins under `plugins/experimental/` (preview skills under active evaluation).

## Build

When you modify skills, run the agentic-workflows build script to validate and regenerate compiled artifacts.

```powershell
pwsh agentic-workflows/<plugin>/build.ps1
```

This validates skill frontmatter and recompiles knowledge lock files. Always commit the regenerated lock files together with your changes.

## Skill-Validator

Don't care much about backwards-compatibility for this tool. Consumers understand that the shape is constantly changing.
