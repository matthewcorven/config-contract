namespace ConfigContract.Abstractions;

public sealed record ContractDiagnostic(
  string Code,
  ContractDiagnosticSeverity Severity,
  string Message,
  ContractSourceLocation? Location = null);