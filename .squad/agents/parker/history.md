# Project Context

- **Owner:** Matthew Corven
- **Project:** config-contract
- **Stack:** C#, .NET 10, xUnit, Microsoft.Extensions.*, Markdown docs
- **Description:** .NET-first configuration contract library for pre-binding validation, diagnostics, sensitivity metadata, and bounded Varlock-spec ingestion
- **Created:** 2026-03-30T15:48:06Z

## Core Context

- Team configured with a small, engineering-heavy cast for a proof-oriented .NET library workflow.
- The core model lives in `src/ConfigContract.Abstractions/` and `src/ConfigContract/`; compatibility work stays behind `src/ConfigContract.VarlockSpec/`.
- Default engineering loop is `dotnet restore`, `dotnet build`, and `dotnet test` against `ConfigContract.sln`.
- Public claims stay narrow and must be backed by automated proofs.

## Learnings

- Initial configured team setup completed on 2026-03-30T15:48:06Z.
- 2026-03-30: The first MVP path stays .NET-only; keep Node, Bun, JavaScript, analyzers, source generation, and broad tooling out of the initial surface.
- 2026-03-30: Repo-level planning docs should mirror the approved MVP PRD and limit hosting language to the current proved DI seam.
- 📌 Team update (2026-03-31T01:44:43Z): Keep core validation on `ContractRegistry` over `IReadOnlyDictionary<string, string?>`, keep hosting at the proved `AddConfigContract` DI seam, and treat exact Varlock root-marker handling as part of the bounded adapter contract. — decided by Parker, Lambert, Dallas, Ripley
