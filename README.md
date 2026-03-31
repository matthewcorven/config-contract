# ConfigContract

ConfigContract is a .NET-first repository for configuration contract work: define the contract for configuration values before binding, validate it, preserve diagnostics and sensitivity metadata, and expose the result through the repo's current registry and the basic `AddConfigContract()`/`ContractRegistry` DI seam.

This repo starts intentionally narrow. The approved near-term scope is the MVP described in [docs/mvp-product-requirements.md](docs/mvp-product-requirements.md): prove a small .NET-first core, the basic `AddConfigContract()`/`ContractRegistry` DI seam, and a bounded Varlock migration lane without broadening public claims. The staged sequencing for that work lives in [docs/roadmap.md](docs/roadmap.md).

## Why ConfigContract

ConfigContract exists for teams that want configuration to behave more like a declared contract than a loose collection of keys, bind calls, and startup checks.

The intended value is straightforward:

- Make required values, defaults, value kinds, and sensitivity intent explicit in one contract model.
- Catch configuration-shape problems before they disappear into object binding or host startup behavior.
- Preserve diagnostics as first-class output instead of relying only on scattered runtime exceptions or app-specific validation code.
- Fit into normal .NET application setup through dependency-light APIs and the basic `AddConfigContract()`/`ContractRegistry` DI seam rather than replacing the stack with a foreign runtime model.
- Provide a bounded migration path from Varlock-style specs without making compatibility baggage the center of the product.

The implemented baseline is intentionally smaller than that full value proposition. The sections below separate current proof from longer-term product direction.

## Approved MVP

The near-term promise is smaller than the full product direction described in [docs/proposals/0001-config-contract-product-direction.md](docs/proposals/0001-config-contract-product-direction.md). For MVP, ConfigContract is committing to:

- a .NET-first contract model with pre-binding validation and explicit diagnostics
- the basic `AddConfigContract()`/`ContractRegistry` DI seam for the default .NET path
- a bounded Varlock import lane as migration input, with explicit unsupported-case diagnostics
- small runnable examples and automated proofs for that surface
- one fast required pull request lane, with heavier validation lanes deferred

The .NET-only inner loop is the governing contributor and CI path for that MVP surface.

The approved MVP scope and exit criteria live in [docs/mvp-product-requirements.md](docs/mvp-product-requirements.md).

The MVP is not promising analyzers, source generation, a CLI, or full Varlock parity.

## What It Is

- A home for a .NET-native contract model and diagnostics surface.
- A proof-oriented repo for pre-binding configuration validation.
- A place to explore a clean compatibility path from Varlock's `@env-spec` into a .NET-first object model.
- A staging ground for packaging, testing, and distribution choices that should not be hidden inside the broader Varlock monorepo.

## What It Is Not

- Not a secret manager or hosted configuration service.
- Not a replacement for `Microsoft.Extensions.Configuration` or the built-in options-validation stack.
- Not a claim that the full Varlock engine, plugin system, or CLI behavior is being reimplemented here on day one.
- Not the canonical place for broad cross-language features; the first milestones are .NET-first and proof-driven.

## When To Reach For It

ConfigContract is worth using when:

- You want a pre-binding contract layer for configuration shape, required versus optional behavior, defaults, or sensitivity metadata.
- You want diagnostics that can travel cleanly through local development, CI, and application startup instead of only post-bind object validation failures.
- You want a .NET-native contract story that fits standard SDK, NuGet, DI, and hosting workflows without bringing in a second runtime.
- You want a bounded migration path from Varlock-style specs into a .NET-owned model.

Built-in .NET validation is often enough when:

- `IConfiguration` binding plus `IOptions<T>`, `ValidateOnStart()`, or `IValidateOptions<T>` already covers the problem.
- You only need post-binding object validation and do not need a separate contract model.
- You do not need sensitivity metadata, import compatibility, or pre-binding diagnostics as a product concern.
- A small amount of application-specific validation code is clearer than introducing a distinct contract layer.

## Current Status

The repo is still intentionally narrow, but it has moved past pure scaffold status. The current baseline now includes a real core validation path, the basic `AddConfigContract()`/`ContractRegistry` DI seam, a bounded Varlock-spec import lane, and automated proofs that exercise those behaviors.

Current, proven baseline:

- The core `ConfigContract` path now includes contract validation and diagnostics in the main library, rather than only placeholder project structure.
- `ContractRegistry` can collect descriptors and aggregate validation results across the registered contracts.
- `ConfigContract.Hosting` proves the basic `AddConfigContract()`/`ContractRegistry` DI seam through automated resolution of `ContractRegistry` and validation through that path.
- `ConfigContract.VarlockSpec` can import the documented Varlock subset in [docs/varlock-supported-subset.md](docs/varlock-supported-subset.md) and report explicit diagnostics for unsupported or malformed cases instead of silently approximating them.
- Tests now provide automated proof in the repo, and the runnable examples stay aligned to the direct validation, DI seam, and Varlock migration lanes the MVP actually owns.

## Repo Layout

Current layout:

| Path | Purpose |
| ---- | ------- |
| `ConfigContract.sln` | Root solution for the repo |
| `global.json` | Pinned .NET SDK baseline |
| `Directory.Build.props` | Shared build defaults |
| `Directory.Packages.props` | Central package versions |
| `src/ConfigContract.Abstractions/` | Core shared contract and diagnostic types |
| `src/ConfigContract/` | Main contract library |
| `src/ConfigContract.Hosting/` | Basic `AddConfigContract()`/`ContractRegistry` DI seam |
| `src/ConfigContract.VarlockSpec/` | Varlock-spec ingestion boundary |
| `src/ConfigContract.Generation/` | Reserved seam for deferred generation work |
| `src/ConfigContract.Analyzers/` | Reserved seam for deferred analyzer work |
| `tests/ConfigContract.Tests/` | Automated proof tests for core validation and Varlock import behavior |
| `examples/` | Narrow, product-owned example set for the new repo |
| `README.md` | Repo overview and working boundaries |
| `AGENTS.md` | Concise development conventions for humans and coding agents |
| `.editorconfig` | Formatting defaults |
| `.gitignore` | Local artifact and secret-file ignores |
| `docs/roadmap.md` | Staged plan for initial milestones |
| `docs/varlock-supported-subset.md` | Exact supported Varlock subset and diagnostic boundary |
| `docs/example-migration-plan.md` | Policy for migrating or rewriting examples from Varlock |

## Build And Test

The current baseline builds and tests through the root solution. The default inner loop is the governing path for contributors and the required pull request lane:

```bash
dotnet restore ConfigContract.sln
dotnet build ConfigContract.sln
dotnet test ConfigContract.sln
```

Guidance for early work:

- Prefer the `dotnet` CLI as the default inner loop and required CI path.
- Keep Node or Bun off the critical path unless the task is specifically about Varlock compatibility fixtures or ingestion comparison.
- Treat every public claim as incomplete until it has a named automated proof.

The current proofs stay intentionally small and specific:

- `tests/ConfigContract.Tests/ContractRegistryTests.cs` proves descriptor storage and duplicate-field validation through the registry path.
- `tests/ConfigContract.Tests/VarlockSpecImporterTests.cs` proves both a supported import fixture and an explicit unsupported root-decorator failure.
- The examples under `examples/` are runnable seeds for the direct registry path, the `AddConfigContract()`/`ContractRegistry` DI seam, and the bounded Varlock import path.

The initial example set is intentionally small. See [examples/README.md](examples/README.md) for the current seeds and [docs/example-migration-plan.md](docs/example-migration-plan.md) for the migration policy from the broader Varlock example matrix.

## Relationship To Varlock Spec Ingestion

ConfigContract is not a fork of Varlock. Varlock remains the existing engine and the current source of truth for `@env-spec` behavior.

This repo exists to answer a narrower question: can a .NET-first contract model consume Varlock-style schema input in a way that is explicit, testable, and useful inside normal .NET configuration flows?

The dedicated `src/ConfigContract.VarlockSpec/` project is the intended compatibility boundary for that work. It should stay separate from the core contract model so the core can be reasoned about and tested without taking parser or runtime dependencies.

Today that compatibility lane is intentionally bounded. The exact supported subset and diagnostic posture are documented in [docs/varlock-supported-subset.md](docs/varlock-supported-subset.md). Anything outside that written subset should be treated as unsupported in the current lane.

That has a few immediate consequences:

- Varlock spec ingestion is an input path, not a promise of full engine parity on day one.
- Unsupported or partially supported `@env-spec` features should be surfaced explicitly, not approximated silently.
- Early compatibility work should be driven by imported fixtures and proof cases from Varlock, not by aspirational docs.
- If a behavior still depends on the Varlock repo or CLI for proof, say so directly.

## Near-Term Priorities

See [docs/mvp-product-requirements.md](docs/mvp-product-requirements.md) for the approved promise and exit criteria, and [docs/roadmap.md](docs/roadmap.md) for sequencing. The immediate priorities are:

1. Protect the MVP path end to end: contract model, diagnostics, registry validation, and the `AddConfigContract()`/`ContractRegistry` DI seam.
2. Keep Varlock spec ingestion bounded to the documented migration subset, with explicit unsupported-case diagnostics.
3. Protect developer experience with one fast required pull request lane; keep larger parity sweeps, performance work, and broader scenario matrices out of the required path for now.
4. Expand tooling, packaging, and example breadth only after the MVP promise is proven and explicitly re-scoped.
