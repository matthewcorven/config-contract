using ConfigContract.Abstractions;

namespace ConfigContract;

public static class ContractValidator
{
  public static ContractValidationResult Validate(ContractDescriptor descriptor)
  {
    ArgumentNullException.ThrowIfNull(descriptor);

    var diagnostics = new List<ContractDiagnostic>();

    if (descriptor.Fields.Count == 0)
    {
      diagnostics.Add(new ContractDiagnostic(
        "CC0001",
        ContractDiagnosticSeverity.Error,
        $"Contract '{descriptor.Name}' must declare at least one field."));
    }

    var fieldNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    foreach (var field in descriptor.Fields)
    {
      if (string.IsNullOrWhiteSpace(field.Name))
      {
        diagnostics.Add(new ContractDiagnostic(
          "CC0002",
          ContractDiagnosticSeverity.Error,
          $"Contract '{descriptor.Name}' contains a field with no name."));
        continue;
      }

      if (!fieldNames.Add(field.Name))
      {
        diagnostics.Add(new ContractDiagnostic(
          "CC0003",
          ContractDiagnosticSeverity.Error,
          $"Contract '{descriptor.Name}' contains a duplicate field '{field.Name}'."));
      }
    }

    return new ContractValidationResult(diagnostics);
  }
}