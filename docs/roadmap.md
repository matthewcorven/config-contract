# ConfigContract Roadmap

## Purpose

This roadmap keeps the first public claims narrow. ConfigContract should earn scope in stages, with each stage ending in runnable proof rather than prose alone.

## Current Baseline

The repo already has the basic scaffold in place:

- `ConfigContract.sln`
- `.NET 10` pinned through `global.json`
- Shared build defaults and central package management
- Project placeholders under `src/`
- A baseline test project under `tests/`

That means the roadmap starts after bootstrap. The remaining work is to turn the scaffold into a credible product surface.

## Working Assumptions

- The repo is .NET-first.
- Varlock spec ingestion matters, but it starts as compatibility work, not a parity promise.
- Pre-binding contract validation is the focus; post-binding application invariants still belong to normal .NET validation.
- Public claims should follow proven behavior, not lead it.

## Example Strategy

ConfigContract should not start by copying the full `.NET` example matrix from the Varlock repo. The early examples in this repository should prove ConfigContract's own identity:

- a minimal direct-inspection or registry example
- a hosting baseline that shows ordinary .NET registration and consumption
- a bounded compatibility example for Varlock spec ingestion

Broader host, reload, cloud, and security examples should be rewritten into this repo only after the corresponding product surfaces exist here. The detailed carry-over policy lives in [docs/example-migration-plan.md](example-migration-plan.md).

## Stage 1: Core Contract Model

Goal: define the in-memory model before choosing how much of Varlock to ingest.

Deliverables:

- Core types for keys, value kinds, defaults, required or optional state, sensitivity or public visibility, and diagnostics.
- Normalization and diagnostic behavior with parser-independent tests.
- Explicit model boundaries for what belongs in the core versus adapters.

Exit criteria:

- The contract model can represent the first planned scenarios without parser code.
- Diagnostics are stable enough to assert in tests.
- No dependency on the Varlock CLI or repo is required for core unit tests.

## Stage 2: Bounded Varlock Spec Ingestion

Goal: accept a documented subset of Varlock `@env-spec` as an input path.

Deliverables:

- Ingestion adapter for a bounded, written-down subset.
- Imported fixtures or mirrored proof cases from Varlock.
- Explicit diagnostics for unsupported or deferred constructs.
- Parity notes that say which features are in, out, or partial.

Exit criteria:

- Supported ingestion cases pass against imported fixtures.
- Unsupported cases fail explicitly and predictably.
- The repo can state exactly what "Varlock-compatible" means at this stage.

## Stage 3: .NET Integration Surface

Goal: flow validated contract data into ordinary .NET configuration consumption.

Deliverables:

- Adapter into `IConfiguration` and options-friendly surfaces.
- Proof apps or targeted integration tests for the default path.
- Clear error reporting around contract or ingestion failures.
- Documentation that separates pre-binding contract validation from post-binding app validation.

Exit criteria:

- A small .NET app can consume the contract through a standard configuration path.
- Failure modes are asserted in automated tests.
- The documented happy path is fully runnable from this repo.

## Stage 4: Generation And Analyzer Surface

Goal: add generation and analyzer work only after the contract and hosting path are stable.

Deliverables:

- Clear generation scope for emitted types or helper artifacts.
- Analyzer rules that reinforce supported usage without inventing unsupported behavior.
- Tests that pin both happy-path output and failure diagnostics.

Exit criteria:

- Generated output is deterministic enough to snapshot or text-assert.
- Analyzer messages are explicit, actionable, and covered by tests.
- The repo can explain why generation or analyzer features belong here instead of in Varlock proper.

## Stage 5: Packaging And Distribution

Goal: make the repo consumable outside local development.

Deliverables:

- NuGet packaging plan and versioning approach.
- Minimal external-consumer proof.
- Release notes template and support statement.
- Docs that explain when to use ConfigContract versus Varlock directly.

Exit criteria:

- A clean consumer outside the repo can restore, build, and run against the published package layout.
- Packaging and support claims are backed by automation, not manual inspection.

## Not In The First Milestones

- Full Varlock engine parity.
- Broad plugin or secret-provider support.
- Non-.NET-first runtime targets.
- Multiple host integrations before the default configuration path is proven.

## Review Rhythm

Revisit this roadmap whenever a milestone changes shape. If scope grows, add proof obligations before adding broader claims.