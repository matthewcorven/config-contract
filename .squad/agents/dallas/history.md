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
- 📌 Team update (2026-03-31T01:44:43Z): Varlock compatibility claims must point to `docs/varlock-supported-subset.md`, stay explicitly lossy, and refuse field materialization on unsupported, conflicting, or raw unsupported input. — decided by Dallas, Parker, Lambert, Ripley
