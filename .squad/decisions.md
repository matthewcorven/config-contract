# Squad Decisions

## Active Decisions

### 2026-03-30: Squad bootstrap is coordination only
**By:** Ripley
**What:** Initialize the squad workspace and guidance as repo coordination infrastructure only. This does not change ConfigContract package boundaries, milestone claims, or the core-versus-compatibility split.
**Why:** The bootstrap touches repo-wide guidance, but the product remains the same .NET-first contract model with bounded Varlock compatibility. Public scope still needs explicit proof before expansion.

### 2026-03-30: Approved MVP stays narrow, .NET-first, and PRD-led
**By:** Ripley, Parker, Lambert, Brett
**What:** Keep the MVP centered on .NET-first pre-binding configuration contract validation, with `docs/mvp-product-requirements.md` as the near-term source of truth. README, roadmap, proposal 0001, repository architecture, and examples must align to that narrower scope, keep one fast required pull request gate in the MVP, and avoid implying analyzers, source generation, a CLI, or full Varlock parity.
**Why:** This keeps the initial value proofable, protects CI runtime and DX constraints, and prevents repo-level planning docs from overcommitting the product shape while broader tooling remains explicitly post-MVP.

### 2026-03-30: MVP core validation stays pre-binding, flat, and dependency-light (consolidated)
**By:** Parker, Lambert, Ripley
**What:** Keep core validation on `ContractRegistry` over `IReadOnlyDictionary<string, string?>`, reject invalid defaults during descriptor validation, and require focused automated proofs for README-visible value-validation and multi-contract aggregation behavior.
**Why:** This closes the MVP gap around required values, defaults, declared value kinds, and registry aggregation without pulling `IConfiguration` into the core package or widening package boundaries.

### 2026-03-30: Hosting stays limited to the proved AddConfigContract DI seam (consolidated)
**By:** Ripley, Lambert, Parker, Brett
**What:** Treat hosting as the small `AddConfigContract()` and `AddConfigContract(Action<ContractRegistry>)` registration seam plus `ContractRegistry` resolution from DI, and describe only that proved behavior in public docs and examples. Keep no-argument DI resolution and registration-time composition backed by focused tests.
**Why:** This keeps docs aligned with the proof set and closes ambiguity around the zero-argument and registration-time seams without widening the story to options binding, startup orchestration, or a broader hosting pipeline.

### 2026-03-30: Varlock compatibility remains a documented, lossy subset with exact root-marker handling (consolidated)
**By:** Dallas, Parker, Lambert, Ripley
**What:** Anchor the compatibility claim to `docs/varlock-supported-subset.md`; accept only the exact no-op `@env-spec` root marker; keep the `@plugin` diagnostic bound to the exact decorator name; do not materialize fields when source lines contain unsupported decorators, conflicting supported decorators, or unsupported raw right-hand-side content. Keep documented unsupported-case and VSP diagnostic proofs focused and mostly inline in `VarlockSpecImporterTests`, reserving fixtures for the import-from-file paths.
**Why:** This keeps the adapter explicitly bounded, prevents silent parity drift or lossy approximation of foreign semantics, and ties the published subset and diagnostic matrix to deterministic proof.

### 2026-03-30: `pr-fast.yml` is the single MVP CI lane
**By:** Parker
**What:** Keep `.github/workflows/pr-fast.yml` as the only MVP CI workflow. It runs on `pull_request`, `push` to `main`, and `workflow_dispatch` with minimal permissions, a simple NuGet cache, and the solution-level `dotnet restore ConfigContract.sln`, `dotnet build ConfigContract.sln -c Release --no-restore`, and `dotnet test ConfigContract.sln -c Release --no-build -v minimal` sequence.
**Why:** This makes the required gate explicit and fast, keeps verification aligned to the approved MVP scope, and avoids broader matrices or example sweeps until the repo needs them.

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
- Hosted branch-protection rules remain an operational setting outside this workspace; verify required checks and approval rules directly on GitHub when release readiness depends on them.
