namespace ConfigContract.Abstractions;

public sealed class ContractValidationResult
{
  public ContractValidationResult(IEnumerable<ContractDiagnostic> diagnostics)
  {
    ArgumentNullException.ThrowIfNull(diagnostics);
    Diagnostics = diagnostics.ToArray();
  }

  public IReadOnlyList<ContractDiagnostic> Diagnostics { get; }

  public bool IsValid => Diagnostics.All(static diagnostic => diagnostic.Severity != ContractDiagnosticSeverity.Error);
}