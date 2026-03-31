# ConfigContract MVP Announcement

ConfigContract has reached its approved MVP checkpoint. The public promise is intentionally narrow: a proof-backed, .NET-first configuration contract surface with a bounded Varlock migration lane.

## MVP Today

- A .NET-first contract model for required or optional values, defaults, value kinds, sensitivity metadata, and explicit diagnostics before binding.
- Pre-binding validation through the registry path, plus the basic `AddConfigContract()` and `ContractRegistry` DI seam for ordinary `Microsoft.Extensions.*` application setup.
- A default developer loop of `dotnet restore ConfigContract.sln`, `dotnet build ConfigContract.sln`, and `dotnet test ConfigContract.sln`, supported by small runnable examples and automated proofs.
- A bounded Varlock migration lane for the documented subset only; unsupported decorators, unsupported right-hand-side content, and broader engine or plugin behavior remain unsupported and are reported explicitly instead of being approximated.

## What This Is Not

This is not an announcement of analyzers, source generation, a CLI, or full Varlock parity. It is not a promise of broader host, reload, cloud, or compatibility coverage beyond the approved MVP surface.

See [README](../README.md), [MVP product requirements](mvp-product-requirements.md), [roadmap](roadmap.md), and [Varlock supported subset](varlock-supported-subset.md).