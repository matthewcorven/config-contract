# ConfigContract

ConfigContract is a .NET-first repository for configuration contract work: define the contract for configuration values before binding, validate it, preserve diagnostics and sensitivity metadata, and feed the result into ordinary .NET configuration and options flows.

This repo starts intentionally narrow. The first job is to prove a small, credible core in its own repository before expanding scope or public claims. The repo already has a minimal .NET 10 solution and test scaffold; implementation follows the staged plan in [docs/roadmap.md](docs/roadmap.md).

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

## Current Status

The repo is brand new, but it is not empty. It already contains a buildable .NET 10 solution, central package management, and a baseline test project. The next step is to fill that scaffold with proven behavior while keeping support claims narrow.

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
| `tests/ConfigContract.Tests/` | Baseline automated tests |
| `examples/` | Narrow, product-owned example set for the new repo |
| `README.md` | Repo overview and working boundaries |
| `AGENTS.md` | Concise development conventions for humans and coding agents |
| `.editorconfig` | Formatting defaults |
| `.gitignore` | Local artifact and secret-file ignores |
| `docs/roadmap.md` | Staged plan for initial milestones |
| `docs/example-migration-plan.md` | Policy for migrating or rewriting examples from Varlock |

## Build And Test

The current scaffold already builds and tests through the root solution. The default inner loop is:

```bash
dotnet restore ConfigContract.sln
dotnet build ConfigContract.sln
dotnet test ConfigContract.sln
```

Guidance for early work:

- Prefer the `dotnet` CLI as the default inner loop.
- Keep Node or Bun off the critical path unless the task is specifically about Varlock compatibility fixtures or ingestion comparison.
- Treat every public claim as incomplete until it has a named automated proof.

The initial example set is intentionally small. See [examples/README.md](examples/README.md) for the current seeds and [docs/example-migration-plan.md](docs/example-migration-plan.md) for the migration policy from the broader Varlock example matrix.

## Relationship To Varlock Spec Ingestion

ConfigContract is not a fork of Varlock. Varlock remains the existing engine and the current source of truth for `@env-spec` behavior.

This repo exists to answer a narrower question: can a .NET-first contract model consume Varlock-style schema input in a way that is explicit, testable, and useful inside normal .NET configuration flows?

The dedicated `src/ConfigContract.VarlockSpec/` project is the intended compatibility boundary for that work. It should stay separate from the core contract model so the core can be reasoned about and tested without taking parser or runtime dependencies.

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