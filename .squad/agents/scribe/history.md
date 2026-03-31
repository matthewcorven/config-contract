# Project Context

- **Owner:** Matthew Corven
- **Project:** config-contract
- **Stack:** C#, .NET 10, xUnit, Microsoft.Extensions.*, Markdown docs
- **Description:** .NET-first configuration contract library for pre-binding validation, diagnostics, sensitivity metadata, and bounded Varlock-spec ingestion
- **Created:** 2026-03-30T15:48:06Z

## Core Context

- Team configured with a small, engineering-heavy cast for a proof-oriented .NET library workflow.
- Shared decisions live in `.squad/decisions.md`; agent-specific learnings live in each agent's `history.md`.
- Default engineering loop is `dotnet restore`, `dotnet build`, and `dotnet test` against `ConfigContract.sln`.

## Learnings

- Initial configured team setup completed on 2026-03-30T15:48:06Z.
- 2026-03-30: When planning narrows MVP scope or CI gates, record the consensus in `.squad/decisions.md`, log the session, and propagate short learnings to the affected agent histories before docs are drafted.
- 2026-03-30: When a docs review blocker is only a proof mismatch, merge the final proof rule into `.squad/decisions.md` and keep the transient rejection out of the active decision log.
- 2026-03-30: When a workflow inbox note records a required CI lane, merge the exact workflow file, trigger set, and verified local command sequence into `.squad/decisions.md`, then clear the inbox note in the same pass.
- 📌 Team update (2026-03-31T01:44:43Z): When recovered reviewer notes converge, keep only the durable end-state decisions in `.squad/decisions.md` and push operational branch-protection reminders into governance notes instead of the active decision list. — decided by Ripley, Parker, Dallas, Lambert, Brett
