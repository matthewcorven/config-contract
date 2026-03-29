using ConfigContract.Abstractions;

namespace ConfigContract;

public sealed class ContractRegistry
{
  private readonly List<ContractDescriptor> _contracts = [];

  public IReadOnlyList<ContractDescriptor> Contracts => _contracts;

  public ContractRegistry Add(ContractDescriptor descriptor)
  {
    ArgumentNullException.ThrowIfNull(descriptor);

    _contracts.Add(descriptor);
    return this;
  }

  public ContractValidationResult Validate()
  {
    var diagnostics = new List<ContractDiagnostic>();
    foreach (var descriptor in _contracts)
    {
      diagnostics.AddRange(ContractValidator.Validate(descriptor).Diagnostics);
    }

    return new ContractValidationResult(diagnostics);
  }
}