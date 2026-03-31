# ConfigContract Repository Architecture

This repository currently supports a narrow .NET-first MVP: pre-binding configuration contract validation, registry behavior, the basic `AddConfigContract()`/`ContractRegistry` DI seam, and bounded Varlock-spec ingestion. It should not be read as the architecture of a broad first-release product line.

## Governing Constraints

### 1. Public scope follows proof

README, proposal, architecture, examples, and package descriptions should claim only the behavior that the repository proves today.

### 2. The default .NET loop and fast required pull request lane govern design

The default developer path and the required CI path are:

```bash
dotnet restore ConfigContract.sln
dotnet build ConfigContract.sln
dotnet test ConfigContract.sln
```

Routine development must stay on that path. Broader compatibility tooling, heavier fixture sweeps, or non-.NET prerequisites stay off the required lane unless a new decision explicitly promotes them.

### 3. The contract model stays dependency-light

The abstractions and the core validation path should remain easy to understand without adapter code, generation code, or external runtime dependencies.

### 4. Compatibility stays at the edge

Varlock ingestion is a migration input, not the architectural center. Unsupported or lossy cases must be reported explicitly.

### 5. Deferred tooling stays deferred

Reserved or placeholder projects are not product commitments. Until a deferred feature has proof and approval, the architecture should describe it as deferred.

## Current Repository Shape

The repository is a small multi-project .NET repo with shared solution and build settings at the root.

Current top-level shape:

```text
docs/
  proposals/
src/
  ConfigContract.Abstractions/
  ConfigContract/
  ConfigContract.Hosting/
  ConfigContract.VarlockSpec/
  ConfigContract.Generation/
  ConfigContract.Analyzers/
tests/
  ConfigContract.Tests/
examples/
  ConfigContract.Example.ConsoleBasic/
  ConfigContract.Example.HostingBasic/
  ConfigContract.Example.VarlockIngestion/
```

The root solution, shared build props, and central package management exist to keep that narrow surface easy to build and validate as one unit.

## Current Package Responsibilities

### `ConfigContract.Abstractions`

Defines the core contract and diagnostic primitives: descriptors, fields, diagnostics, source locations, validation results, and value kinds.

This is the dependency-light center of the repo.

### `ConfigContract`

Owns the core validator and registry behavior.

Today this is the main pre-binding validation path the MVP proves.

### `ConfigContract.Hosting`

Adds the basic `AddConfigContract()`/`ContractRegistry` DI seam.

Today that means registering `ContractRegistry` through the normal `IServiceCollection` path. It is not yet a broader configuration provider or options-integration layer.

### `ConfigContract.VarlockSpec`

Owns the bounded Varlock-spec import lane.

Its job is to translate supported inputs into the ConfigContract model and emit explicit diagnostics for unsupported or malformed constructs.

### `ConfigContract.Generation`

Reserved seam for future generation-related work.

The project exists, but no supported generation feature is part of the approved MVP.

### `ConfigContract.Analyzers`

Reserved seam for future analyzer work.

The project exists, but no analyzer feature is part of the approved MVP.

## Dependency Direction

The current dependency direction is intentionally simple:

- `ConfigContract` depends on `ConfigContract.Abstractions`.
- `ConfigContract.Hosting` depends on `ConfigContract`.
- `ConfigContract.VarlockSpec` depends on `ConfigContract.Abstractions`.
- `ConfigContract.Generation` currently depends on `ConfigContract.Abstractions`, but remains deferred.
- `ConfigContract.Analyzers` currently stands alone as a reserved tooling seam.

The important rule is that compatibility and deferred tooling must not pull extra runtime or JavaScript dependencies into the core path.

## Testing And CI Stance

- `tests/ConfigContract.Tests/` is the current proof set for registry behavior and Varlock import behavior.
- `examples/` stays small and product-owned: direct validation, the basic `AddConfigContract()`/`ContractRegistry` DI seam, and bounded Varlock ingestion.
- The governing CI path is one fast required lane built around `dotnet restore`, `dotnet build`, and `dotnet test` on `ConfigContract.sln`.
- Broader compatibility sweeps, performance work, or large example matrices are deferred until they are approved and affordable.

## Expansion Rules

- New package boundaries or public APIs should appear only when a proved behavior needs them.
- Placeholder projects do not create a support promise.
- If the repo expands into richer configuration or options integration, analyzers, source generation, or CLI tooling, that expansion should update the MVP docs and roadmap in the same change.
- Varlock compatibility may expand only as an explicit support matrix with unsupported cases kept visible.

## What This Repository Should Feel Like

A contributor or adopter should be able to conclude the following:

- ConfigContract is a small, dependency-light .NET library repo first.
- The core model and validation path are the center.
- The basic `AddConfigContract()`/`ContractRegistry` DI seam and bounded Varlock ingestion are proven seams, not marketing placeholders.
- Broader tooling and package growth remain conditional, not assumed.