# Example Migration Plan

## Purpose

This document defines how example coverage should move from the existing Varlock repository into ConfigContract.

The rule is simple: migrate scenario coverage, not project inventory.

ConfigContract should inherit the strongest proof scenarios from the current `.NET` work, but it should not copy the full bridge-era example matrix into a new repository whose product identity is supposed to be .NET-native.

## Migration Policy

### Migrate now

Bring over only examples that support the repo's current milestones and already read like normal .NET usage.

- one console happy-path example
- one hosting baseline example
- one bounded Varlock-spec ingestion example

These examples should be rewritten into ConfigContract shape, not copied mechanically.

### Migrate later

Move broader example families only after the corresponding product surface exists in ConfigContract.

- typed generation and analyzer examples
- stronger options and binding examples
- additional host examples such as worker, MVC, or Functions
- public-only or sensitivity-focused examples
- cloud-provider examples once native provider ownership exists here

### Keep in Varlock for now

Leave bridge- and CLI-specific examples in the Varlock repository.

- explicit executable path
- PATH lookup and local package lookup
- custom working directory or schema path
- direct bridge-runtime plumbing
- live cloud-provider specimens driven by current Varlock plugins
- broad framework coverage that still proves the bridge-based support matrix

### Never treat as first-wave ConfigContract examples

Do not copy examples whose main purpose is to prove legacy acquisition mechanics or JavaScript-era behavior.

- repo-relative CLI discovery
- Node or Bun assumptions in the normal path
- plugin resolution stories that ConfigContract does not yet own natively
- examples that imply full Varlock parity before the compatibility surface is documented and tested

## Current Mapping from Varlock Example Families

| Varlock example family | ConfigContract action | Timing | Notes |
| --- | --- | --- | --- |
| `dotnet-console/` | Rewrite into a ConfigContract console happy path | now | Seed for the default onboarding story |
| `dotnet-console-direct-load/` | Merge concept into native direct-inspection example | now | Keep the scenario, drop bridge wording |
| `dotnet-console-validation/` | Revisit as a failure-path example | later | Only after ConfigContract has real diagnostics |
| `dotnet-console-typed-config/` | Rewrite once generation or binding shape exists here | later | Do not copy bridge-era generated-shape assumptions |
| `dotnet-console-di-options/` | Rewrite into options example | later | Depends on the first credible hosting/config path |
| `dotnet-aspnet-mvc/` | Rewrite into a minimal host example | later | Prefer a minimal host over copied template weight |
| `dotnet-worker/` | Candidate later host example | later | Useful once hosting semantics stabilize |
| `dotnet-functions-isolated/` | Keep in Varlock for now | keep | Too host-specific for first-wave ConfigContract |
| `dotnet-blazor-server/` | Keep in Varlock for now | keep | Revisit after public/sensitive boundary is productized |
| `dotnet-blazor-wasm-public/` | Keep as future reference only | keep | Depends on public-only projection and generation |
| `dotnet-winforms/` | Keep in Varlock for now | keep | Valuable later, but not for the first proof set |
| `dotnet-console-reload/`, `options-snapshot`, `options-monitor` | Keep for later rewrite | later | Only after native runtime/reload behavior exists |
| `dotnet-console-sensitive/`, `public-only/`, `serilog/`, `leak-prevention/` | Keep for later rewrite | later | Requires stronger core metadata and enforcement story |
| `dotnet-console-custom-runtime/`, `explicit-executable/`, `custom-working-dir/`, `custom-schema-path/` | Leave in Varlock | keep | Bridge mechanics, not ConfigContract product identity |
| `dotnet-console-azure-key-vault/` and live cloud-provider specimens | Leave in Varlock | keep | Should only move after native provider ownership exists in this repo |

## Copy vs Rewrite Rule

An example may be copied nearly verbatim only if all of the following are true:

1. It directly supports a current ConfigContract milestone.
2. It does not depend on bridge-specific setup, CLI discovery, or JavaScript toolchain assumptions.
3. Its proof meaning stays the same after package and namespace renaming.
4. It reads like normal `.NET` usage, not migration scaffolding.

Otherwise, rewrite it from scratch and use the old example only as source material.

## First-Wave Example Set for This Repo

The initial example set in this repository should stay small and credible.

- `ConfigContract.Example.ConsoleBasic`
- `ConfigContract.Example.HostingBasic`
- `ConfigContract.Example.VarlockIngestion`

These examples are seeds, not a claim of feature completeness.