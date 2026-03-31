using System.Globalization;
using ConfigContract.Abstractions;

namespace ConfigContract;

public static class ContractValidator
{
  public static ContractValidationResult Validate(ContractDescriptor descriptor)
  {
    ArgumentNullException.ThrowIfNull(descriptor);

    var diagnostics = new List<ContractDiagnostic>();
    ValidateDescriptor(descriptor, diagnostics);

    return new ContractValidationResult(diagnostics);
  }

  internal static ContractValidationResult Validate(ContractDescriptor descriptor, IReadOnlyDictionary<string, string?> values)
  {
    ArgumentNullException.ThrowIfNull(descriptor);
    ArgumentNullException.ThrowIfNull(values);

    var diagnostics = new List<ContractDiagnostic>();
    var validFields = ValidateDescriptor(descriptor, diagnostics);

    foreach (var field in validFields)
    {
      var hasConfiguredValue = values.TryGetValue(field.Name, out var configuredValue) && configuredValue is not null;
      var effectiveValue = hasConfiguredValue ? configuredValue : field.DefaultValue;

      if (effectiveValue is null)
      {
        if (field.Required)
        {
          diagnostics.Add(new ContractDiagnostic(
            "CC0005",
            ContractDiagnosticSeverity.Error,
            $"Contract '{descriptor.Name}' requires a value for field '{field.Name}'."));
        }

        continue;
      }

      if (!IsValidForKind(field.Kind, effectiveValue))
      {
        diagnostics.Add(new ContractDiagnostic(
          "CC0006",
          ContractDiagnosticSeverity.Error,
          $"Contract '{descriptor.Name}' field '{field.Name}' has a {(hasConfiguredValue ? "configured" : "default")} value '{effectiveValue}' that is not a valid {DescribeKind(field.Kind)}."));
      }
    }

    return new ContractValidationResult(diagnostics);
  }

  private static List<ContractField> ValidateDescriptor(ContractDescriptor descriptor, List<ContractDiagnostic> diagnostics)
  {
    if (descriptor.Fields.Count == 0)
    {
      diagnostics.Add(new ContractDiagnostic(
        "CC0001",
        ContractDiagnosticSeverity.Error,
        $"Contract '{descriptor.Name}' must declare at least one field."));

      return [];
    }

    var validFields = new List<ContractField>(descriptor.Fields.Count);
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
        continue;
      }

      if (field.DefaultValue is not null && !IsValidForKind(field.Kind, field.DefaultValue))
      {
        diagnostics.Add(new ContractDiagnostic(
          "CC0004",
          ContractDiagnosticSeverity.Error,
          $"Contract '{descriptor.Name}' field '{field.Name}' declares default value '{field.DefaultValue}' that is not a valid {DescribeKind(field.Kind)}."));
        continue;
      }

      validFields.Add(field);
    }

    return validFields;
  }

  private static bool IsValidForKind(ContractValueKind kind, string value)
  {
    return kind switch
    {
      ContractValueKind.String => true,
      ContractValueKind.Integer => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out _),
      ContractValueKind.Boolean => bool.TryParse(value, out _),
      _ => false,
    };
  }

  private static string DescribeKind(ContractValueKind kind)
  {
    return kind switch
    {
      ContractValueKind.String => "string",
      ContractValueKind.Integer => "integer",
      ContractValueKind.Boolean => "boolean",
      _ => kind.ToString(),
    };
  }
}