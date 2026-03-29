# Examples

This directory contains the first ConfigContract-owned example set.

The examples here are intentionally narrow. They exist to prove the early identity of the new repository, not to mirror the full `.NET` support matrix from the Varlock repo.

## Current Examples

| Example | Purpose |
| --- | --- |
| `ConfigContract.Example.ConsoleBasic` | Smallest direct-inspection and registry usage path |
| `ConfigContract.Example.HostingBasic` | Minimal DI and hosting integration path |
| `ConfigContract.Example.VarlockIngestion` | Bounded compatibility lane for Varlock-spec ingestion |

## Why The Set Is Small

ConfigContract should not start by copying the entire bridge-era example inventory. Doing so would make the new repo read like a relocated adapter instead of a .NET-native product.

The broader example matrix still lives in the Varlock repo. This repo grows examples only when the corresponding product surface is owned here.

See [../docs/example-migration-plan.md](../docs/example-migration-plan.md) for the carry-over policy.