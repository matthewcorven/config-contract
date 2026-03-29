using ConfigContract;
using ConfigContract.Abstractions;
using ConfigContract.Hosting;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddConfigContract();

using var serviceProvider = services.BuildServiceProvider();
var registry = serviceProvider.GetRequiredService<ContractRegistry>();
registry.Add(new ContractDescriptor("WebApp", new[]
{
	new ContractField("APP_NAME", ContractValueKind.String, DefaultValue: "web"),
	new ContractField("APP_PORT", ContractValueKind.Integer, DefaultValue: "8080"),
}));

var validation = registry.Validate();

Console.WriteLine("ConfigContract hosting baseline");
Console.WriteLine($"Registered contracts: {registry.Contracts.Count}");
Console.WriteLine($"Valid: {validation.IsValid}");