# ConfigContract MVP Product Requirements

**Status:** Drafted from approved direction  
**Updated:** 2026-03-30

This document defines the approved near-term MVP for ConfigContract. It is intentionally narrower than the broader product direction in [Proposal 0001](proposals/0001-config-contract-product-direction.md) and the long-term architecture in [repository-architecture.md](repository-architecture.md). The staged delivery sequence lives in [roadmap.md](roadmap.md).

## Product Summary

ConfigContract MVP is a .NET-first configuration contract library surface for declaring configuration expectations before binding, validating those expectations, preserving actionable diagnostics and sensitivity metadata, and fitting into ordinary .NET DI and hosting flows.

## Problem Statement

.NET teams often discover configuration problems too late: after binding has already lost shape information, during host startup, or inside scattered application-specific validation code. The built-in .NET validation stack is useful, but it does not provide a single pre-binding contract model with explicit diagnostics and a bounded migration lane for existing Varlock specs.

ConfigContract MVP exists to prove a smaller claim: a .NET team should be able to define and validate configuration contracts early, get deterministic diagnostics in local development and CI, and do that without introducing a JavaScript toolchain or a large compatibility surface.

## Target User

The MVP is for .NET application teams and library owners who:

- want pre-binding validation for configuration shape, required values, defaults, value kinds, and sensitivity intent
- want explicit diagnostics that travel cleanly through local development, CI, and startup
- may have some existing Varlock specs but need a bounded migration lane into a .NET-first workflow

## Explicit MVP Promise

By the end of the MVP, ConfigContract can publicly promise only the following:

- a .NET-first contract model for required or optional values, defaults, value kinds, sensitivity metadata, and diagnostics
- a validation and registry path that runs before binding and returns explicit, testable diagnostics
- basic hosting integration for the default `Microsoft.Extensions.*` path
- a bounded Varlock-spec import lane for a documented subset, with explicit unsupported-case diagnostics
- small runnable examples and automated proofs for the supported surface
- one fast required pull request lane focused on this MVP surface, with heavier lanes deferred

## In Scope

- the core contract model and diagnostics surface already owned by this repository
- deterministic validation behavior and proof-backed diagnostics
- the default .NET developer path through `dotnet restore`, `dotnet build`, and `dotnet test`
- basic hosting or DI integration that proves the product fits normal .NET application setup
- a bounded Varlock migration lane with written support limits
- concise docs and examples that describe only the proven MVP surface

## Out Of Scope

- analyzers
- source generation
- a first-party CLI
- full Varlock engine parity or broad compatibility claims
- broad host, reload, cloud, or security example matrices
- non-.NET-first runtime targets
- heavy CI gates as part of the required pull request path

## Developer Experience Requirements

- The default developer workflow must stay .NET-only and must not require Node.js, Bun, npm, or another JavaScript runtime.
- The README, roadmap, and examples must point to the same narrow MVP story instead of mixing near-term claims with long-term product direction.
- Diagnostics should be explicit enough to assert in tests and readable enough to act on in CI output.
- Examples should stay small, product-owned, and directly mapped to the MVP surface: direct validation, hosting integration, and the bounded Varlock migration lane.
- Unsupported behavior must be called out directly instead of implied through roadmap or example sprawl.

## CI And Runtime Requirements

The MVP must optimize for fast routine pull requests and predictable runtime behavior.

Required pull request lane:

- One fast required lane should cover restore, build, and fast deterministic proofs for the MVP surface.
- That required lane should protect the direct validation path, the default hosting path, and the bounded Varlock migration lane.
- The required lane should stay small enough to be routine protection, not a catch-all validation program.

Deferred heavier lanes:

- Larger compatibility sweeps, broader example matrices, performance runs, and similar deeper validation can exist as non-required or post-MVP lanes.
- Those heavier lanes must not become part of the default required path until the MVP proves they are necessary and affordable.
- Runtime behavior claims should be backed by the fast required lane first, then expanded only when new lanes and support statements are approved.

## Success Metrics And Exit Criteria

The MVP is ready to treat as the repo's near-term public promise when:

- every public MVP claim has an automated proof in this repository
- a new .NET contributor can build and test the repo with the default .NET SDK workflow alone
- the small example set stays aligned to the MVP surface and remains runnable
- unsupported Varlock constructs are diagnosed explicitly rather than approximated silently
- the required pull request lane protects the supported surface without pulling in deferred heavy lanes
- public docs do not imply analyzers, source generation, a CLI, or full Varlock parity are part of the MVP

## Risks To Watch

- Compatibility creep could turn the bounded Varlock lane into an implied parity promise unless unsupported behavior stays explicit.
- CI lane creep could erode the fast default path if broader fixture sets or heavier jobs are added to required pull request validation too early.

## Approval-Sensitive Questions

No blocking approval questions remain for this draft. Future scope expansion should be treated as a new approval decision rather than an edit to this MVP promise.
