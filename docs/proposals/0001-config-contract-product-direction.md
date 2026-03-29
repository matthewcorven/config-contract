# Proposal 0001: ConfigContract Product Direction

**Status:** Proposed  
**Created:** 2026-03-30

## Decision Summary

ConfigContract will be built as a standalone, fully .NET-native product line for defining, validating, and consuming configuration contracts in .NET applications.

Varlock is relevant to this product only as a compatibility and migration lane. ConfigContract may ingest Varlock specifications, but that ingestion path is an adapter boundary, not the product identity, not the architectural center, and not the driver of public API design.

This repository is the dedicated home for ConfigContract. It will be a .NET monorepo containing the core model, runtime, integrations, tooling, examples, and compatibility adapters required to ship a serious .NET product.

## Why This Exists

The product need is larger than a bridge from one ecosystem into another.

Teams want configuration to behave like a contract:

- declared explicitly
- validated consistently
- typed for application code
- aware of sensitive values and operational policy
- portable across local development, CI, and production environments

That product needs a native .NET identity if it is going to feel coherent to .NET teams. A wrapper around a JavaScript runtime cannot be the long-term center of gravity for that experience.

## Product Vision

ConfigContract is the .NET product for contract-defined application configuration.

Its job is to let a .NET team define configuration once, enforce it consistently, and consume it through normal .NET application patterns without introducing a second-class runtime model.

The intended end state is:

- a first-party contract model owned by ConfigContract
- first-class .NET APIs for runtime loading, validation, diagnostics, and typed access
- first-class integration with `IConfiguration`, `IOptions`, hosting, source generation, analyzers, and build tooling
- first-class support for sensitivity metadata, environment overlays, and operational guardrails
- a compatibility lane for importing existing Varlock specs without letting that lane define the core platform

ConfigContract should be recognizable as a .NET product even in teams that have never used Varlock.

## What Fully .NET-Native Means

"Fully .NET-native" is a hard product direction, not marketing language.

It means:

1. The canonical runtime is implemented in C# and runs without Node.js, Bun, npm, or any JavaScript host.
2. The canonical public APIs follow .NET idioms, naming, packaging, diagnostics, and lifecycle expectations.
3. The product ships through normal .NET channels such as NuGet packages, a `dotnet tool`, SDK integration, and standard CI/build workflows.
4. The core authoring, validation, and consumption model is owned by ConfigContract and is not defined by JavaScript package behavior.
5. The reference examples, tests, performance work, and release criteria are driven by .NET scenarios first.
6. Any compatibility with external ecosystems is additive and isolated; it does not control the architecture of the core product.

In practical terms, a .NET team should be able to adopt ConfigContract using only the .NET SDK, NuGet, and standard .NET tooling.

## Relationship to Varlock Spec Ingestion

ConfigContract may support Varlock spec ingestion because there is real migration value in meeting existing users where they are. That support is useful, but it is intentionally narrow.

The relationship is:

- Varlock specs are one possible input format.
- Imported Varlock specs are translated into the ConfigContract model.
- The compatibility layer lives in a dedicated adapter package and test lane.
- Unsupported or lossy semantics are diagnosed explicitly instead of being hidden behind silent behavior.
- Core packages do not depend on Varlock package structure, naming, or release cadence.

This means ConfigContract is not a rebranded Varlock bridge. It is a native product that can accept Varlock input when that is useful.

## Product Identity Rules

To keep the product identity clear, the following rules apply:

1. The root product, package, and namespace name is `ConfigContract`.
2. Core packages must not be branded as Varlock-compatible packages first.
3. Public documentation must describe Varlock as an import or migration path, not as the primary reason the product exists.
4. The first-party contract model and first-party .NET APIs are the center of the design.
5. Compatibility packages may use names such as `ConfigContract.VarlockCompat`, but that naming must stay outside the core runtime and authoring packages.

## Proposed Repository and Package Architecture

This repository should be a dedicated .NET monorepo with multiple packages that version together until there is strong evidence that independent versioning is worth the added complexity.

The initial package map should be:

- `ConfigContract.Model`
  - Canonical contract object model, diagnostics primitives, metadata, and shared value abstractions.
- `ConfigContract.Authoring`
  - First-party contract authoring formats, parsers, serializers, and authoring-time validation.
- `ConfigContract.Runtime`
  - Core resolution engine, environment overlays, sensitivity semantics, and compiled contract loading.
- `ConfigContract.Configuration`
  - `Microsoft.Extensions.Configuration` provider and binding helpers.
- `ConfigContract.Hosting`
  - `HostApplicationBuilder` and `WebApplicationBuilder` integration plus DI registration.
- `ConfigContract.SourceGeneration`
  - Source generators for typed configuration accessors and metadata projections.
- `ConfigContract.Analyzers`
  - Roslyn analyzers and code fixes for contract authoring and runtime consumption.
- `ConfigContract.Cli`
  - Validation, inspection, export, migration, and CI-facing commands distributed as a `dotnet tool`.
- `ConfigContract.VarlockCompat`
  - Varlock spec ingestion and translation into the ConfigContract model.

The dependency direction should stay simple:

- `ConfigContract.Model` is the stable center.
- Authoring and compatibility packages produce the model.
- Runtime consumes the model.
- Configuration and hosting packages sit on top of runtime.
- Source generation, analyzers, and CLI depend on the model and relevant upper layers, but core runtime must not depend on them.

## Monorepo vs Split Decision for This Repository

This repository should start as a monorepo.

That is the correct choice for this product because:

- the packages share one domain model and one compatibility story
- source generation, analyzers, runtime behavior, and examples must evolve together
- the early release cadence should prioritize coherence over independent package movement
- compatibility work needs to be tested against the same core model and diagnostics contracts as first-party features
- documentation, examples, and migration guidance need to move in lockstep with package changes

This is not a monorepo because of JavaScript legacy. It is a monorepo because ConfigContract is a product line with several closely related .NET deliverables.

Split repositories should be considered only if a package develops a clearly independent release cadence, contributor surface, and support policy.

## Hard Boundaries Around JavaScript Compatibility

These boundaries are mandatory.

1. Core ConfigContract packages must not require Node.js, Bun, npm, pnpm, or a JavaScript runtime for normal build, test, or production use.
2. Core runtime behavior must not be implemented by shelling out to a JavaScript executable.
3. Public API design must not be constrained by JavaScript naming, package structure, or bug-for-bug compatibility.
4. JavaScript or Varlock compatibility code must live in dedicated adapter packages and dedicated compatibility tests.
5. Compatibility imports must translate into ConfigContract semantics. If translation is incomplete, the product must emit diagnostics instead of pretending parity exists.
6. Release decisions for ConfigContract are driven by .NET product needs, not by the feature velocity or backlog shape of a JavaScript codebase.
7. Documentation must not present JavaScript compatibility as the default onboarding path.

## Non-Goals

The first phase of ConfigContract is not trying to do the following:

- become a thin wrapper over the Varlock CLI
- preserve bug-for-bug behavior with JavaScript implementations
- publish mirrored npm packages from this repository
- make JavaScript parity a gating condition for core .NET runtime progress
- let imported spec syntax dictate the long-term shape of the ConfigContract object model

## Phased Implementation Plan

### Phase 0: Foundation

Establish the repository as a .NET product line.

- create the solution, build layout, package conventions, CI skeleton, and docs baseline
- define the canonical contract model and diagnostics strategy
- define versioning, package boundaries, and test taxonomy

### Phase 1: Core Model and Native Authoring

Build the product center before compatibility work expands.

- implement `ConfigContract.Model`
- implement first-party authoring and validation in `ConfigContract.Authoring`
- define how contracts are represented, normalized, and serialized inside the product

### Phase 2: Runtime and .NET Integration

Make the product useful to application developers.

- implement `ConfigContract.Runtime`
- ship `ConfigContract.Configuration` and `ConfigContract.Hosting`
- support typed access patterns, sensitivity metadata, and environment-specific evaluation

### Phase 3: Tooling and Developer Experience

Improve authoring and adoption.

- ship source generation and analyzers
- ship the `ConfigContract.Cli` `dotnet tool`
- add examples for console, ASP.NET Core, worker, and test scenarios

### Phase 4: Varlock Compatibility Lane

Add migration value without changing the product center.

- implement `ConfigContract.VarlockCompat`
- define the supported Varlock import surface explicitly
- add compatibility diagnostics, fixtures, and migration guides

### Phase 5: Hardening and Expansion

Scale the product once the foundations are real.

- performance tuning and caching
- richer policy features and operational controls
- broader platform examples and enterprise integration points

## Success Criteria

This direction is succeeding when all of the following are true:

- a .NET team can adopt ConfigContract without installing any JavaScript toolchain
- the core runtime and primary developer experience are understandable without knowing Varlock
- Varlock ingestion exists as a migration lane with explicit support boundaries
- package boundaries remain clean enough that compatibility work cannot distort the core runtime architecture
- the repository reads like the home of a .NET product line rather than an adapter layer

## Final Position

ConfigContract is a .NET-native configuration contract product.

Varlock ingestion is useful, but it is a lane into the product, not the product itself.