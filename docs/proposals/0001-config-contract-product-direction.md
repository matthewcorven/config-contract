# Proposal 0001: ConfigContract Product Direction

**Status:** Proposed direction, bounded by approved MVP
**Created:** 2026-03-30
**Updated:** 2026-03-30

## Decision Summary

ConfigContract should continue as a dedicated .NET-first repository, but the approved near-term claim is intentionally narrow: prove a small pre-binding configuration contract library for .NET with explicit diagnostics, basic DI and hosting integration, and a bounded Varlock migration lane.

This proposal does not approve a broad first release spanning runtime layers, configuration or options integration, source generation, analyzers, CLI tooling, or a large package map. Those are deferred possibilities. They require separate proof, explicit re-scoping, and a new approval decision before they become part of the product promise.

Varlock remains relevant only as a compatibility and migration input. That lane is useful, but it is not the product center, not the package center, and not the driver of API design.

## Why This Exists

ConfigContract exists because .NET teams often need a stricter, earlier configuration contract than ordinary binding and startup checks provide.

The product need proven by the MVP is deliberately small:

- define required values, defaults, value kinds, and sensitivity intent explicitly
- validate those expectations before binding obscures shape
- return diagnostics that can be asserted in tests and acted on in CI
- keep the default developer path inside the .NET SDK and normal `Microsoft.Extensions.*` patterns
- provide a bounded migration lane for existing Varlock specs without making compatibility semantics the center of the product

## Approved Near-Term Direction

The approved near-term direction is:

- one .NET-first contract model owned by ConfigContract
- validation and registry behavior that runs before binding and returns explicit diagnostics
- basic DI and hosting integration for the default `Microsoft.Extensions.*` path
- bounded Varlock-spec ingestion for a documented subset, with explicit unsupported-case diagnostics
- small runnable examples and automated proofs for only that surface
- one fast required pull request lane focused on the MVP surface

The default developer loop and the required pull request lane are governing constraints, not side notes. Routine development and routine CI must continue to work through:

```bash
dotnet restore ConfigContract.sln
dotnet build ConfigContract.sln
dotnet test ConfigContract.sln
```

Node.js, Bun, npm, and broader compatibility tooling stay off that critical path unless a specific compatibility task requires them.

## What .NET-First Means Right Now

For the approved MVP, ".NET-first" means:

1. The canonical implementation and the required CI path run with the .NET SDK and do not require a JavaScript runtime.
2. The public API centers on ConfigContract's own contract and diagnostic model, not imported compatibility semantics.
3. The default onboarding path is the direct .NET path, not a Varlock bridge.
4. Documentation describes only the surface the repository proves today.

That is a narrower claim than "fully featured .NET product line," and it should stay narrower until the MVP is proven and re-approved.

## Relationship To Varlock Spec Ingestion

ConfigContract may ingest Varlock specs because there is real migration value in meeting existing users where they are. That support stays intentionally bounded.

The relationship is:

- Varlock specs are one input format, not the core authoring model.
- Imported specs translate into ConfigContract's own contract model.
- The compatibility lane lives in a dedicated adapter package and test area.
- Unsupported or lossy semantics must be diagnosed explicitly instead of being hidden behind silent behavior.
- Core package boundaries and public APIs do not depend on Varlock package structure, naming, or release cadence.

This means ConfigContract is not a rebranded Varlock bridge. It is a .NET-first library with a bounded migration lane.

## Current Repository Stance

The current committed repository shape is intentionally small.

Committed MVP surface:

- `ConfigContract.Abstractions` for contract and diagnostic primitives
- `ConfigContract` for registry and validation behavior
- `ConfigContract.Hosting` for the basic DI and hosting seam
- `ConfigContract.VarlockSpec` for bounded Varlock ingestion
- one shared test project and a small example set that prove those behaviors

Reserved or deferred seams:

- `ConfigContract.Generation` may hold future generation-related contracts, but it is not part of the MVP promise.
- `ConfigContract.Analyzers` may hold future analyzer work, but it is not part of the MVP promise.

Placeholder or reserved projects must not be treated as approval for a broader first release.

## Deferred Expansion

The following areas are explicitly deferred until the MVP is proven and a new direction decision approves the expansion:

- richer configuration or options integration beyond the current basic hosting seam
- first-party authoring formats or parsers beyond today's direct model construction
- source generation
- analyzers
- a first-party CLI
- broader host, reload, cloud, or security example matrices
- larger package splits or a versioned product map beyond the current proven surface

These items are potential expansion lanes, not current commitments.

## Non-Goals

The near-term ConfigContract direction is not trying to:

- become a thin wrapper over the Varlock CLI or runtime
- preserve bug-for-bug parity with JavaScript implementations
- require Node.js, Bun, npm, or another JavaScript toolchain for normal development or production use
- describe deferred tooling as if it were part of the current public product
- turn the required pull request lane into a broad product-validation program before the MVP proves that cost is warranted

## Success Criteria

This direction is succeeding when all of the following are true:

- a .NET team can build, test, and understand the supported surface with the default .NET SDK workflow alone
- the required pull request lane stays fast and deterministic
- public docs read like a narrow, proof-backed .NET library, not a broad unproven product line
- Varlock ingestion remains a bounded migration lane with explicit support limits
- expansion work is described as deferred until the repo has proof and approval to widen the promise

## Final Position

ConfigContract is a .NET-first repository with a narrow, proof-backed MVP.

Broader runtime, configuration, tooling, and packaging expansion may happen later, but none of that is committed by this proposal today.