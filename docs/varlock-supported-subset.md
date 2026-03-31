# Varlock Supported Subset

**Status:** Current bounded compatibility lane for `ConfigContract.VarlockSpec`

This document defines the exact Varlock-style subset that ConfigContract imports today. It is a support boundary for a migration input lane, not a parity statement for the Varlock engine, plugin system, or CLI.

Support is intentionally fixture-driven and test-backed. If a construct is not listed here, treat it as unsupported in the current compatibility lane.

## Supported Today

Supported input shape:

- One optional root marker: `@env-spec` on a recognized comment decorator line. It is accepted as a no-op compatibility marker and does not become part of the core contract model.
- Line-based config item declarations in the form `KEY = ...`.
- Exactly one supported type decorator per field: `@string`, `@integer`, or `@boolean`.
- Optional field decorators: `@required`, `@sensitive`, and one `@default(...)`.

Imported semantics:

- Supported lines become `ContractField` entries in a `ContractDescriptor`.
- `@default(...)` is carried as the imported default string value. The importer trims outer whitespace and removes surrounding double quotes when present.
- Imported fields use ConfigContract's own value kinds and diagnostics model; Varlock-specific semantics do not flow into the core packages.

Current proven happy path:

```text
# @env-spec

APP_NAME = @string @default("config-contract")
APP_PORT = @integer @default(5000)
DATABASE_URL = @string @required
API_KEY = @string @sensitive
```

## Explicit Unsupported Boundary

The current lane does not promise broad Varlock compatibility.

- Any root decorator other than `@env-spec` is unsupported.
- Any field decorator not listed in the supported set above is unsupported.
- Any extra right-hand-side content beyond the supported decorators is unsupported.
- Multiple supported type decorators on one field are unsupported.
- Multiple `@default(...)` decorators on one field are unsupported.
- A field line without a supported type decorator is unsupported.
- The adapter does not claim plugin behavior, CLI behavior, or full engine parity.

## Diagnostic Posture

The importer fails explicitly instead of silently approximating foreign semantics.

- Unsupported or malformed input produces `Error` diagnostics with `VSP` codes and source locations.
- Unsupported root decorators diagnose the file, but other supported field lines may still import.
- A field line with unsupported, conflicting, or lossy content is omitted from the imported descriptor instead of being partially materialized.
- Unsupported cases currently diagnose with these codes:

| Code | Condition |
| ---- | --------- |
| `VSP0001` | Line is not a supported config item declaration |
| `VSP0002` | Config item key is empty |
| `VSP0003` | Field does not contain any supported Varlock type token |
| `VSP0004` | Field is missing a supported type decorator after token processing |
| `VSP0005` | Field contains unsupported right-hand-side content |
| `VSP0006` | Field declares multiple supported type decorators |
| `VSP0007` | Field declares multiple `@default(...)` decorators |
| `VSP1001` | Field decorator is unsupported in the current lane |
| `VSP2001` | Root decorator `@plugin(...)` is outside the supported lane |
| `VSP2002` | Any other root decorator is unsupported |

## Proof Sources

The documented subset is currently backed by the importer implementation and the focused fixture/test set in this repository:

- `tests/ConfigContract.Tests/Fixtures/Varlock/basic.env.schema`
- `tests/ConfigContract.Tests/Fixtures/Varlock/unsupported-plugin.env.schema`
- `tests/ConfigContract.Tests/VarlockSpecImporterTests.cs`

If support expands, update this document in the same change as the implementation and proof.