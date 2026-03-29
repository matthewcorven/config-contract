# ConfigContract

ConfigContract is a .NET-first repository for configuration contract work: define the contract for configuration values before binding, validate it, preserve diagnostics and sensitivity metadata, and feed the result into ordinary .NET configuration and options flows.

This repo starts intentionally narrow. The first job is to prove a small, credible core in its own repository before expanding scope or public claims. The repo already has a minimal .NET 10 solution and test scaffold; implementation follows the staged plan in [docs/roadmap.md](docs/roadmap.md).

## Why ConfigContract

ConfigContract exists for teams that want configuration to behave more like a declared contract than a loose collection of keys, bind calls, and startup checks.

The intended value is straightforward:

- Make required values, defaults, value kinds, and sensitivity intent explicit in one contract model.
- Catch configuration-shape problems before they disappear into object binding or host startup behavior.
- Preserve diagnostics as first-class output instead of relying only on scattered runtime exceptions or app-specific validation code.
- Fit into normal .NET configuration, options, DI, and hosting flows rather than replacing them with a foreign runtime model.
- Provide a bounded migration path from Varlock-style specs without making compatibility baggage the center of the product.

The implemented baseline is intentionally smaller than that full value proposition. The sections below separate current proof from longer-term product direction.

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
- You want a .NET-native contract story that fits standard SDK, NuGet, configuration, and hosting workflows.
- You want a bounded migration path from Varlock-style specs into a .NET-owned model.

Built-in .NET validation is often enough when:

- `IConfiguration` binding plus `IOptions<T>`, `ValidateOnStart()`, or `IValidateOptions<T>` already covers the problem.
- You only need post-binding object validation and do not need a separate contract model.
- You do not need sensitivity metadata, import compatibility, or pre-binding diagnostics as a product concern.
- A small amount of application-specific validation code is clearer than introducing a distinct contract layer.

## Current Status

The repo is still intentionally narrow, but it has moved past pure scaffold status. The current baseline now includes a real core validation path, a bounded Varlock-spec import lane, and automated proofs that exercise those behaviors.

Current, proven baseline:

- The core `ConfigContract` path now includes contract validation and diagnostics in the main library, rather than only placeholder project structure.
- `ContractRegistry` can collect descriptors and aggregate validation results across the registered contracts.
- `ConfigContract.VarlockSpec` can import a documented first-wave subset of Varlock-style schema input and report explicit diagnostics for unsupported or malformed cases instead of silently approximating them.
- Tests and runnable examples now prove behavior in the repo, rather than only proving that the solution scaffold exists.

## Repo Layout

Current layout:

| Path | Purpose |
| ---- | ------- |
| `ConfigContract.sln` | Root solution for the repo |
| `global.json` | Pinned .NET SDK baseline |
| `Directory.Build.props` | Shared build defaults |
| `Directory.Packages.props` | Central package versions |
| `src/ConfigContract.Abstractions/` | Core shared types and interfaces |
| `src/ConfigContract/` | Main contract library |
| `src/ConfigContract.Hosting/` | Host and configuration integration surface |
| `src/ConfigContract.VarlockSpec/` | Varlock-spec ingestion boundary |
| `src/ConfigContract.Generation/` | Generation-related code |
| `src/ConfigContract.Analyzers/` | Analyzer or diagnostic tooling |
| `tests/ConfigContract.Tests/` | Automated proof tests for core validation and Varlock import behavior |
| `examples/` | Narrow, product-owned example set for the new repo |
| `README.md` | Repo overview and working boundaries |
| `AGENTS.md` | Concise development conventions for humans and coding agents |
| `.editorconfig` | Formatting defaults |
| `.gitignore` | Local artifact and secret-file ignores |
| `docs/roadmap.md` | Staged plan for initial milestones |
| `docs/example-migration-plan.md` | Policy for migrating or rewriting examples from Varlock |

## Build And Test

The current baseline builds and tests through the root solution. The default inner loop is:

```bash
dotnet restore ConfigContract.sln
dotnet build ConfigContract.sln
dotnet test ConfigContract.sln
```

Guidance for early work:

- Prefer the `dotnet` CLI as the default inner loop.
- Keep Node or Bun off the critical path unless the task is specifically about Varlock compatibility fixtures or ingestion comparison.
- Treat every public claim as incomplete until it has a named automated proof.

The current proofs stay intentionally small and specific:

- `tests/ConfigContract.Tests/ContractRegistryTests.cs` proves descriptor storage and duplicate-field validation through the registry path.
- `tests/ConfigContract.Tests/VarlockSpecImporterTests.cs` proves both a supported import fixture and an explicit unsupported root-decorator failure.
- The examples under `examples/` are runnable seeds for the direct registry path, the hosting path, and the bounded Varlock import path.

The initial example set is intentionally small. See [examples/README.md](examples/README.md) for the current seeds and [docs/example-migration-plan.md](docs/example-migration-plan.md) for the migration policy from the broader Varlock example matrix.

## Relationship To Varlock Spec Ingestion

ConfigContract is not a fork of Varlock. Varlock remains the existing engine and the current source of truth for `@env-spec` behavior.

This repo exists to answer a narrower question: can a .NET-first contract model consume Varlock-style schema input in a way that is explicit, testable, and useful inside normal .NET configuration flows?

The dedicated `src/ConfigContract.VarlockSpec/` project is the intended compatibility boundary for that work. It should stay separate from the core contract model so the core can be reasoned about and tested without taking parser or runtime dependencies.

Today that compatibility lane is intentionally bounded. It accepts the small line-based subset exercised by the included fixtures and examples, and it emits explicit diagnostics when a construct falls outside the current lane.

That has a few immediate consequences:

- Varlock spec ingestion is an input path, not a promise of full engine parity on day one.
- Unsupported or partially supported `@env-spec` features should be surfaced explicitly, not approximated silently.
- Early compatibility work should be driven by imported fixtures and proof cases from Varlock, not by aspirational docs.
- If a behavior still depends on the Varlock repo or CLI for proof, say so directly.

## Near-Term Priorities

See [docs/roadmap.md](docs/roadmap.md) for the staged plan. The first milestones are:

1. Define the core contract model and diagnostics surface without taking parser dependencies.
2. Add a bounded Varlock ingestion lane with explicit parity fixtures and unsupported-case reporting.
3. Prove the hosting and configuration path into normal .NET configuration and options usage.
4. Add generation, analyzer, and packaging work only after the core path is proven.
5. Grow the example set by rewriting the highest-value scenarios, not by bulk-copying the bridge-era project matrix.