# F# 9 Compiler Breaking Changes (.NET 9)

These breaking changes affect F# projects targeting `net9.0` (which uses F# 9 by default). F# 9 ships with the .NET 9 SDK.

## Source-Incompatible Changes

### Auto-generated `.Is*` properties on discriminated unions

**Impact: Medium.** F# 9 auto-generates `.Is*` properties for each case of a discriminated union. If you already defined custom `Is*` properties or members with the same names, a conflict will occur.

```fsharp
// BREAKS — custom property conflicts with auto-generated one
type Shape =
    | Circle of radius: float
    | Rectangle of width: float * height: float
    member this.IsCircle = // now conflicts with auto-generated IsCircle
        match this with Circle _ -> true | _ -> false
```

**Fix:** Remove the custom `Is*` members — the compiler-generated versions provide the same functionality.

### Struct unions with overlapping fields and reflection

**Impact: Low–Medium.** In FSharp.Core 9.0, struct unions with overlapping fields now generate detailed internal mappings to support correct reading via reflection. Code or libraries using `FSharpValue.GetUnionFields` or similar reflection APIs on struct unions may see different behavior or exceptions if they relied on the previous incomplete mapping.

**Fix:** Update libraries that reflect over struct unions. The new mapping is more complete and correct.

### `ArgumentOutOfRangeException` for collection index out-of-bounds

**Impact: Low.** Accessing an out-of-bounds index in FSharp.Core collections (e.g., `Array`, `List`) now throws `System.ArgumentOutOfRangeException` instead of `System.ArgumentException` in some cases. If your exception-handling code specifically catches `ArgumentException` and not `ArgumentOutOfRangeException`, update it.

```fsharp
// Before: threw ArgumentException in some cases
// After: throws ArgumentOutOfRangeException
try
    let _ = [1; 2; 3].[10]
    ()
with
| :? System.ArgumentOutOfRangeException -> () // update catch patterns
```

## New Language Features (non-breaking but relevant)

### Nullable reference type support (opt-in)

F# 9 adds support for nullable reference types, but this is **off by default**. Enable with `<Nullable>enable</Nullable>` in the `.fsproj` file. Enabling this may surface new warnings about null usage in existing code.

### `_.Property` shorthand for member access in lambdas

F# 9 introduces `_.Property` shorthand syntax in pipelines:
```fsharp
customers |> List.map _.Name
```

This is purely additive and does not break existing code.
