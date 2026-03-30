using ConfigContract.Hosting;
using ConfigContract.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigContract.Tests;

public sealed class ContractRegistryTests
{
  [Fact]
  public void AddStoresDescriptor()
  {
    var registry = new ContractRegistry();
    var descriptor = new ContractDescriptor("App", new[]
    {
      new ContractField("APP_NAME", ContractValueKind.String, DefaultValue: "config-contract"),
    });

    registry.Add(descriptor);

    var stored = Assert.Single(registry.Contracts);
    Assert.Same(descriptor, stored);
  }

  [Fact]
  public void ValidateReturnsErrorForDuplicateFieldNames()
  {
    var registry = new ContractRegistry()
      .Add(new ContractDescriptor("App", new[]
      {
        new ContractField("APP_NAME", ContractValueKind.String),
        new ContractField("APP_NAME", ContractValueKind.String),
      }));

    var result = registry.Validate();

    Assert.False(result.IsValid);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("CC0003", diagnostic.Code);
  }

  [Fact]
  public void AddConfigContractRegistersContractRegistryAndResolvedServiceValidatesContracts()
  {
    var services = new ServiceCollection();
    services.AddConfigContract();

    using var serviceProvider = services.BuildServiceProvider();
    var registry = serviceProvider.GetRequiredService<ContractRegistry>();

    registry.Add(new ContractDescriptor("App", new[]
    {
      new ContractField("APP_NAME", ContractValueKind.String),
      new ContractField("APP_NAME", ContractValueKind.String),
    }));

    var result = registry.Validate();

    Assert.False(result.IsValid);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("CC0003", diagnostic.Code);
  }
}