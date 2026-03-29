using ConfigContract.Abstractions;

namespace ConfigContract.VarlockSpec;

public sealed class VarlockSpecImportResult
{
  public VarlockSpecImportResult(ContractDescriptor descriptor, IEnumerable<ContractDiagnostic> diagnostics)
  {
    ArgumentNullException.ThrowIfNull(descriptor);
    ArgumentNullException.ThrowIfNull(diagnostics);

    Descriptor = descriptor;
    Diagnostics = diagnostics.ToArray();
  }

  public ContractDescriptor Descriptor { get; }

  public IReadOnlyList<ContractDiagnostic> Diagnostics { get; }

  public bool IsSuccessful => Diagnostics.All(static diagnostic => diagnostic.Severity != ContractDiagnosticSeverity.Error);
}