namespace ConfigContract.Abstractions;

public sealed record ContractSourceLocation(string? FilePath, int? Line, int? Column);