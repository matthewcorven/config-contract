# ConfigContract Conventions

## Default Stance

- Treat this as a .NET-first repository. Prefer the `dotnet` CLI, standard SDK projects, and `Microsoft.Extensions.*` abstractions.
- Keep claims narrow. If a feature is planned but not implemented and proven, describe it as planned.
- Do not add `.squad` files. Coordination lives in docs, issues, and pull requests.

## Layout

- Put product code in `src/`.
- Put automated tests in `tests/`.
- Put design notes and milestone planning in `docs/`.
- Add runnable proofs only when they support a specific public claim.

## Tooling

- Use the existing root solution as the default entry point for build and test work.
- Keep the default developer loop to `dotnet restore`, `dotnet build`, and `dotnet test`.
- Avoid introducing Node or Bun into the main build unless the task is specifically about Varlock spec ingestion or compatibility proofs.

## C# Conventions

- Enable nullable reference types and implicit usings unless a project has a concrete reason not to.
- Prefer small, composable libraries over a single large assembly.
- Prefer explicit types when they improve readability; avoid cleverness.
- Multi-target only when a real host or packaging requirement demands it and tests cover the extra target.

## Testing And Proof

- Every externally stated behavior needs an automated proof.
- Unit tests should stay fast and deterministic.
- Integration or proof tests must name the exact scenario they prove.
- Varlock ingestion work must include parity fixtures and explicit unsupported-case tests.

## Dependencies

- Add external packages only for a concrete, documented need.
- Prefer `Microsoft.Extensions.*` primitives before custom abstractions.
- Keep source generation, packaging, or host-specific helpers separate from the core contract model when possible.

## Documentation

- Update `README.md` and `docs/roadmap.md` when scope or milestone shape changes.
- Keep unsupported behavior explicit.
- If a design choice changes a public claim, update the proof plan in the same change.