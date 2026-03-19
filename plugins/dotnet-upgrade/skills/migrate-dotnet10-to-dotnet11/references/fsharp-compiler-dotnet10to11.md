# F# 11 Compiler Breaking Changes (.NET 11)

These breaking changes affect F# projects targeting `net11.0` (which uses F# 11 by default). F# 11 ships with the .NET 11 SDK.

> **Note:** .NET 11 is in preview. This covers changes through Preview 1. Additional breaking changes may be introduced in later previews.

## Source-Incompatible Changes

### ML compatibility removal

**Impact: Medium.** F# 11 removes all remaining ML/OCaml compatibility constructs that the compiler has carried since F#'s origins as an OCaml dialect. This is a significant cleanup (~7,000 lines of legacy code removed).

The following are **removed**:
- **Source file extensions:** `.ml` and `.mli` files are no longer recognized as F# source files.
- **Directives:** `#light` and `#indent` directives are removed. (Whitespace-sensitive syntax has been the default for many years.)
- **Compiler flags:** `--mlcompatibility`, `--light`, `--indentation-syntax`, `--no-indentation-syntax`, and `--ml-keywords` are all removed.
- **Reserved keywords released:** `asr`, `land`, `lor`, `lsl`, `lsr`, and `lxor` — previously reserved for ML compatibility — are now available as regular identifiers.

```fsharp
// BREAKS — .ml file extension
// Rename MyModule.ml → MyModule.fs
// Rename MyModule.mli → MyModule.fsi

// BREAKS — #light directive
#light "off"  // error: directive no longer recognized

// FIX — simply remove the directive (whitespace-sensitive syntax is the default)
```

**Fix:**
1. Rename any `.ml` files to `.fs` and `.mli` files to `.fsi`.
2. Remove all `#light` and `#indent` directives from source files.
3. Remove `--mlcompatibility` and related flags from project files, build scripts, and CI configurations.
4. If you used `asr`, `land`, `lor`, `lsl`, `lsr`, or `lxor` as escaped identifiers (e.g., `` ``asr`` ``), you can now use them as plain identifiers.

See also: [dotnet/fsharp#19143](https://github.com/dotnet/fsharp/pull/19143)

## Performance Improvements (non-breaking)

### Parallel compilation enabled by default

Parallel compilation (preview in F# 10) is now enabled by default for all projects. This includes parallel reference resolution, graph-based type checking, parallel optimizations, and parallel IL code generation.

If you encounter issues, opt out with the `--parallelcompilation-` compiler flag.

### Faster compilation of computation expression-heavy code

The compiler's stack-overflow prevention mechanism (`StackGuard`) has been replaced with `RuntimeHelpers.TryEnsureSufficientExecutionStack()`, significantly reducing thread creation for deeply nested computation expressions (e.g., `task { }`, `async { }`).
