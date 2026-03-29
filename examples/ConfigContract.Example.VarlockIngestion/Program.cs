using ConfigContract;
using ConfigContract.Abstractions;
using ConfigContract.VarlockSpec;

var samplePath = Path.Combine(AppContext.BaseDirectory, "Samples", "basic.env.schema");
var importResult = VarlockSpecImporter.ImportFromFile(samplePath);
var registry = new ContractRegistry().Add(importResult.Descriptor);
var validation = registry.Validate();

Console.WriteLine("ConfigContract Varlock ingestion baseline");
Console.WriteLine($"Imported descriptor: {registry.Contracts[0].Name}");
Console.WriteLine($"Fields imported: {registry.Contracts[0].Fields.Count}");
Console.WriteLine($"Import diagnostics: {importResult.Diagnostics.Count}");
Console.WriteLine($"Valid: {validation.IsValid}");