using ConfigContract;
using ConfigContract.Abstractions;

var registry = new ContractRegistry()
  .Add(new ContractDescriptor("App", new[]
  {
    new ContractField("APP_NAME", ContractValueKind.String, DefaultValue: "config-contract"),
    new ContractField("APP_PORT", ContractValueKind.Integer, DefaultValue: "5000"),
  }))
  .Add(new ContractDescriptor("Database", new[]
  {
    new ContractField("DATABASE_URL", ContractValueKind.String, Required: true, Sensitive: true),
  }));

var configuredValues = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
{
  ["APP_PORT"] = "5000",
  ["DATABASE_URL"] = "Host=localhost;Database=config-contract;",
};

var validation = registry.Validate(configuredValues);

Console.WriteLine("ConfigContract console baseline");
Console.WriteLine($"Valid: {validation.IsValid}");
Console.WriteLine($"Diagnostics: {validation.Diagnostics.Count}");
foreach (var contract in registry.Contracts)
{
  Console.WriteLine($"- {contract.Name}: {contract.Fields.Count} fields");
}