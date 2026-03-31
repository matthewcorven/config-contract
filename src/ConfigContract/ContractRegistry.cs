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

  public ContractValidationResult Validate(IReadOnlyDictionary<string, string?> values)
  {
    ArgumentNullException.ThrowIfNull(values);

    var normalizedValues = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
    foreach (var pair in values)
    {
      if (string.IsNullOrWhiteSpace(pair.Key))
      {
        continue;
      }

      normalizedValues[pair.Key] = pair.Value;
    }

    var diagnostics = new List<ContractDiagnostic>();
    foreach (var descriptor in _contracts)
    {
      diagnostics.AddRange(ContractValidator.Validate(descriptor, normalizedValues).Diagnostics);
    }

    return new ContractValidationResult(diagnostics);
  }
}