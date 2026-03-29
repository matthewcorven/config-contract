# Examples

This directory contains the first ConfigContract-owned example set.

The examples here are intentionally narrow. They exist to prove the early identity of the new repository, not to mirror the full `.NET` support matrix from the Varlock repo.

## Current Examples

| Example | Purpose |
| --- | --- |
| `ConfigContract.Example.ConsoleBasic` | Manually constructs descriptors, adds them to a registry, runs validation, and prints the resulting contract summary |
| `ConfigContract.Example.HostingBasic` | Registers ConfigContract through `Microsoft.Extensions.DependencyInjection`, resolves `ContractRegistry`, and validates a host-owned contract |
| `ConfigContract.Example.VarlockIngestion` | Imports a supported Varlock schema sample, adds the imported descriptor to the registry, and shows the import-diagnostics count alongside contract validation |

These are real seeds now, not placeholder project shells. Each example exercises a concrete baseline path that the repo currently owns: direct registry validation, basic hosting registration, or bounded Varlock-spec ingestion.

## Why The Set Is Small

ConfigContract should not start by copying the entire bridge-era example inventory. Doing so would make the new repo read like a relocated adapter instead of a .NET-native product.

The broader example matrix still lives in the Varlock repo. This repo grows examples only when the corresponding product surface is owned here.

See [../docs/example-migration-plan.md](../docs/example-migration-plan.md) for the carry-over policy.