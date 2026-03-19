# F# 10 Compiler Breaking Changes (.NET 10)

These breaking changes affect F# projects targeting `net10.0` (which uses F# 10 by default). F# 10 ships with the .NET 10 SDK.

## Source-Incompatible Changes

### `#nowarn` / `#warnon` directive syntax tightened

**Impact: Medium.** F# 10 introduces `#warnon` for scoped warning suppression and tightens the syntax rules for warning directives:

- **Multiline and empty `#nowarn` directives are disallowed.**
- **Whitespace between `#` and `nowarn` is disallowed** (e.g., `# nowarn` must be `#nowarn`).
- **Triple-quoted, interpolated, or verbatim strings for warning numbers are disallowed.**
- **In scripts (`.fsx`):** `#nowarn` now applies only until the end of the file or a corresponding `#warnon`, rather than affecting the whole script.

```fsharp
// BREAKS — multiline #nowarn
#nowarn
    "20"

// BREAKS — whitespace between # and nowarn
# nowarn "20"

// BREAKS — triple-quoted string
#nowarn """20"""

// FIX — standard single-line syntax
#nowarn "20"
```

**Fix:** Update all `#nowarn` directives to use the standard single-line format with plain string literals.

### Module-in-type structural validation

**Impact: Low–Medium.** The compiler now raises an error when a `module` declaration appears indented at the same structural level inside a type definition. This was previously accepted but led to confusing scoping behavior.

```fsharp
// BREAKS — module inside type at same indentation level
type MyType() =
    member _.X = 1
module Helpers =   // error: module cannot appear here
    let helper () = ()

// FIX — move module to outer scope
type MyType() =
    member _.X = 1

module Helpers =
    let helper () = ()
```

### Access modifiers on auto property getters/setters

**Impact: Low.** F# 10 allows different access modifiers for auto property getters and setters (e.g., `member val X = 0 with public get, private set`). This can cause conflicts if a computation expression builder defines members that clash with the new syntax parsing.

**Fix:** If compilation fails in CE builder code after upgrading, review property accessor definitions for naming conflicts.

## New Language Features (non-breaking but relevant)

### Scoped warning suppression (`#warnon`)

F# 10 introduces `#warnon` to re-enable a previously suppressed warning within the same file:
```fsharp
#nowarn "20"
// ... code where warning 20 is suppressed ...
#warnon "20"
// ... warning 20 is active again ...
```

### Union case access modifiers

Discriminated union cases can now have access modifiers:
```fsharp
type Result<'T> =
    | Ok of 'T
    | internal Error of string
```

### `ValueOption` optional parameters

Optional parameters can now use `[<Struct>] ValueOption<'T>` to reduce heap allocations.

### Parallel compilation (preview)

Parallel compilation is available as a preview feature with `LangVersion=Preview` and `Deterministic=false`. This becomes the default in .NET 11.
