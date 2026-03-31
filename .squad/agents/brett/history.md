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
- 2026-03-30: PRD framing should emphasize narrow .NET-first value, developer experience, and CI runtime, and stop short of product-doc writing before proposal approval.
- 2026-03-30: Once `docs/mvp-product-requirements.md` is approved, treat it as the near-term source of truth and keep README and roadmap wording inside the proved MVP and CI and DX constraints.
- 📌 Team update (2026-03-31T01:44:43Z): Public docs and examples must stay inside the approved MVP, describe only the proved `AddConfigContract` and `ContractRegistry` seam, and link to `docs/varlock-supported-subset.md` for the bounded adapter claim. — decided by Brett, Dallas, Ripley, Parker, Lambert
