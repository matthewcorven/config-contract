namespace ConfigContract.Abstractions;

public sealed record ContractField(
  string Name,
  ContractValueKind Kind,
  bool Required = false,
  bool Sensitive = false,
  string? DefaultValue = null);