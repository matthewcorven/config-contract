# ConfigContract Roadmap

## Purpose

This roadmap sequences the approved MVP without widening the public promise. The MVP definition and exit criteria live in [docs/mvp-product-requirements.md](mvp-product-requirements.md); this document describes how work should be staged around that target.

## Current Baseline

The repo is already past pure scaffold status. Today it includes:

- a core contract model and validation path
- registry aggregation for contract validation results
- the basic `AddConfigContract()`/`ContractRegistry` DI seam through `ConfigContract.Hosting`
- a bounded Varlock-spec importer with explicit diagnostics for unsupported or malformed input
- automated tests and small runnable examples for the direct validation, DI seam, and migration lanes

That means the roadmap is about tightening and proving the MVP, not inventing a broader first release.

## Working Assumptions

- The repo is .NET-first, and the default developer loop remains `dotnet restore`, `dotnet build`, and `dotnet test` against `ConfigContract.sln`.
- The MVP prioritizes developer experience and CI runtime over feature breadth.
- Varlock spec ingestion is a bounded migration lane, not a parity program.
- Public wording follows automated proof.
- Broader tooling or compatibility expansion must wait until the MVP surface is proven.

## Required PR Lane

The MVP needs one fast required pull request lane.

That lane should:

- cover restore, build, and fast deterministic proofs for the MVP surface
- protect the direct validation path, the `AddConfigContract()`/`ContractRegistry` DI seam, and the bounded Varlock migration lane
- stay small enough that routine pull requests are not blocked by broad fixture matrices or slow environment setup

Heavier lanes are intentionally deferred from required pull request gating during MVP. Larger compatibility sweeps, performance checks, broader example matrices, and similar work can exist as non-required or post-MVP lanes once the narrow surface is stable.

## Example Strategy

The examples in this repository should continue to prove only the product surface the repo owns today:

- a minimal direct validation example
- a minimal `AddConfigContract()`/`ContractRegistry` DI example
- a bounded Varlock migration example

Broader host, reload, cloud, and security examples stay out of the MVP until the corresponding product surface exists and has proof in this repository. The carry-over policy remains in [docs/example-migration-plan.md](example-migration-plan.md).

## Stage 1: Stabilize The Narrow Core

Goal: finish the smallest credible product surface promised in the MVP PRD.

Deliverables:

- stable contract-model and diagnostic behavior for the default .NET path
- registry validation behavior with explicit, testable diagnostics
- the basic `AddConfigContract()`/`ContractRegistry` DI seam that proves ConfigContract fits ordinary .NET registration and consumption flows
- a documented bounded Varlock subset in [varlock-supported-subset.md](varlock-supported-subset.md) with explicit unsupported-case reporting
- README, roadmap, and example positioning aligned to the same narrow promise

Exit criteria:

- each public MVP claim is backed by an automated proof in this repository
- the runnable examples stay small and .NET-only
- unsupported or deferred Varlock constructs fail explicitly instead of being approximated
- the default developer path does not require a JavaScript toolchain

## Stage 2: Lock The Fast Developer Loop

Goal: make the narrow MVP cheap to work on and cheap to protect in pull requests.

Deliverables:

- one fast required pull request lane that covers the MVP surface
- heavier compatibility or broader-scenario lanes split out of the required path
- CI and local developer guidance that favor the default .NET loop over additional tooling

Exit criteria:

- the required lane stays focused on the MVP surface rather than future backlog items
- heavier lanes can run without turning every pull request into a broad product-validation sweep
- developer documentation describes the fast path first

## Deferred Beyond MVP

These items are explicitly outside the MVP and should not be described as near-term commitments in public docs:

- analyzers
- source generation
- a first-party CLI
- full Varlock parity or broad compatibility claims
- broad host, reload, cloud, or security example matrices
- packaging or distribution promises beyond what the narrow MVP proves

## Review Rhythm

Revisit this roadmap when the MVP exit criteria change or when the repo has proof that a deferred lane deserves promotion into the default path.
