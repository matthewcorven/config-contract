# Examples

This directory contains the small ConfigContract-owned example set for the approved MVP story.

The examples here are intentionally narrow. They exist to illustrate the approved MVP surface and stay aligned with the automated proof set, not to mirror the full `.NET` support matrix from the Varlock repo.

## Current Examples

| Example | Purpose |
| --- | --- |
| `ConfigContract.Example.ConsoleBasic` | Manually constructs descriptors, adds them to a registry, runs validation, and prints the resulting contract summary |
| `ConfigContract.Example.HostingBasic` | Calls `AddConfigContract()`, resolves `ContractRegistry`, and validates a host-owned contract through the default DI seam |
| `ConfigContract.Example.VarlockIngestion` | Imports a supported Varlock schema sample, adds the imported descriptor to the registry, and shows the import-diagnostics count alongside contract validation |

These are real seeds now, not placeholder project shells. Each example exercises a concrete MVP lane that the repo currently owns: direct registry validation, the `AddConfigContract()`/`ContractRegistry` DI seam, or bounded Varlock-spec ingestion.

## Why The Set Is Small

ConfigContract should not start by copying the entire bridge-era example inventory. Doing so would make the new repo read like a relocated adapter instead of a .NET-native product.

The broader example matrix still lives in the Varlock repo. This repo grows examples only when the corresponding product surface is owned and proof-backed here.

See [../docs/example-migration-plan.md](../docs/example-migration-plan.md) for the carry-over policy.