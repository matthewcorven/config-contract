# Squad Decisions

## Active Decisions

### 2026-03-30: Squad bootstrap is coordination only
**By:** Ripley
**What:** Initialize the squad workspace and guidance as repo coordination infrastructure only. This does not change ConfigContract package boundaries, milestone claims, or the core-versus-compatibility split.
**Why:** The bootstrap touches repo-wide guidance, but the product remains the same .NET-first contract model with bounded Varlock compatibility. Public scope still needs explicit proof before expansion.

### 2026-03-30: Approved MVP stays narrow, .NET-first, and PRD-led
**By:** Ripley, Parker, Lambert, Brett
**What:** Keep the MVP centered on .NET-first pre-binding configuration contract validation, with `docs/mvp-product-requirements.md` as the near-term source of truth. README, roadmap, proposal 0001, and repository architecture must align to that narrower scope, keep one fast required pull request gate in the MVP, and avoid implying analyzers, source generation, a CLI, or full Varlock parity.
**Why:** This keeps the initial value proofable, protects CI runtime and DX constraints, and prevents repo-level planning docs from overcommitting the product shape while broader tooling remains explicitly post-MVP.

### 2026-03-30: Hosting claims stay limited to the proved DI seam
**By:** Ripley, Lambert
**What:** Treat hosting as a basic `AddConfigContract()` and `ContractRegistry` DI seam in the MVP and require an automated proof on that path before describing hosting as proof-backed.
**Why:** This keeps the docs aligned to the current test surface and closes the last mismatch without widening the hosting story beyond what the repository currently implements and tests.

### 2026-03-30: `pr-fast.yml` is the single MVP CI lane
**By:** Parker
**What:** Keep `.github/workflows/pr-fast.yml` as the only MVP CI workflow. It runs on `pull_request`, `push` to `main`, and `workflow_dispatch` with minimal permissions, a simple NuGet cache, and the solution-level `dotnet restore ConfigContract.sln`, `dotnet build ConfigContract.sln -c Release --no-restore`, and `dotnet test ConfigContract.sln -c Release --no-build -v minimal` sequence.
**Why:** This makes the required gate explicit and fast, keeps verification aligned to the approved MVP scope, and avoids broader matrices or example sweeps until the repo needs them.

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
