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
  public void ValidateAggregatesDiagnosticsAcrossMultipleContracts()
  {
    var registry = new ContractRegistry()
      .Add(new ContractDescriptor("App", new[]
      {
        new ContractField("APP_NAME", ContractValueKind.String),
        new ContractField("APP_NAME", ContractValueKind.String),
      }))
      .Add(new ContractDescriptor("Worker", new[]
      {
        new ContractField("RETRY_COUNT", ContractValueKind.Integer, DefaultValue: "not-an-integer"),
      }));

    var result = registry.Validate();

    Assert.False(result.IsValid);
    Assert.Collection(
      result.Diagnostics,
      diagnostic =>
      {
        Assert.Equal("CC0003", diagnostic.Code);
        Assert.Contains("App", diagnostic.Message, StringComparison.Ordinal);
      },
      diagnostic =>
      {
        Assert.Equal("CC0004", diagnostic.Code);
        Assert.Contains("Worker", diagnostic.Message, StringComparison.Ordinal);
      });
  }

  [Fact]
  public void ValidateWithValuesReturnsSuccessForMatchingConfiguredAndDefaultValues()
  {
    var registry = new ContractRegistry()
      .Add(new ContractDescriptor("App", new[]
      {
        new ContractField("APP_NAME", ContractValueKind.String, Required: true),
        new ContractField("RETRY_COUNT", ContractValueKind.Integer, DefaultValue: "3"),
        new ContractField("FEATURE_ENABLED", ContractValueKind.Boolean, DefaultValue: "true"),
      }));

    var result = registry.Validate(new Dictionary<string, string?>
    {
      ["app_name"] = "config-contract",
    });

    Assert.True(result.IsValid);
    Assert.Empty(result.Diagnostics);
  }

  [Fact]
  public void ValidateWithValuesReturnsMissingRequiredValueDiagnosticWhenNoValueOrDefaultExists()
  {
    var registry = new ContractRegistry()
      .Add(new ContractDescriptor("App", new[]
      {
        new ContractField("APP_NAME", ContractValueKind.String, Required: true),
      }));

    var result = registry.Validate(new Dictionary<string, string?>());

    Assert.False(result.IsValid);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("CC0005", diagnostic.Code);
    Assert.Contains("APP_NAME", diagnostic.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ValidateWithValuesReturnsInvalidConfiguredValueDiagnostic()
  {
    var registry = new ContractRegistry()
      .Add(new ContractDescriptor("App", new[]
      {
        new ContractField("RETRY_COUNT", ContractValueKind.Integer),
      }));

    var result = registry.Validate(new Dictionary<string, string?>
    {
      ["RETRY_COUNT"] = "not-an-integer",
    });

    Assert.False(result.IsValid);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("CC0006", diagnostic.Code);
    Assert.Contains("configured value", diagnostic.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ValidateWithValuesReturnsInvalidDefaultValueDiagnostic()
  {
    var registry = new ContractRegistry()
      .Add(new ContractDescriptor("App", new[]
      {
        new ContractField("RETRY_COUNT", ContractValueKind.Integer, DefaultValue: "not-an-integer"),
      }));

    var result = registry.Validate(new Dictionary<string, string?>());

    Assert.False(result.IsValid);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("CC0004", diagnostic.Code);
    Assert.Contains("default value", diagnostic.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void AddConfigContractWithoutConfigureActionRegistersUsableRegistry()
  {
    var descriptor = new ContractDescriptor("App", new[]
    {
      new ContractField("APP_NAME", ContractValueKind.String, Required: true),
    });

    var services = new ServiceCollection();
    services.AddConfigContract();

    using var serviceProvider = services.BuildServiceProvider();
    var registry = serviceProvider.GetRequiredService<ContractRegistry>();

    registry.Add(descriptor);

    var result = registry.Validate(new Dictionary<string, string?>
    {
      ["APP_NAME"] = "config-contract",
    });

    Assert.Same(descriptor, Assert.Single(registry.Contracts));
    Assert.True(result.IsValid);
    Assert.Empty(result.Diagnostics);
  }

  [Fact]
  public void AddConfigContractWithConfigureActionRegistersConfiguredRegistryThatValidatesValues()
  {
    var descriptor = new ContractDescriptor("App", new[]
    {
      new ContractField("APP_NAME", ContractValueKind.String, Required: true),
      new ContractField("RETRY_COUNT", ContractValueKind.Integer, DefaultValue: "3"),
    });

    var services = new ServiceCollection();
    services.AddConfigContract(registry => registry.Add(descriptor));

    using var serviceProvider = services.BuildServiceProvider();
    var registry = serviceProvider.GetRequiredService<ContractRegistry>();

    Assert.Same(descriptor, Assert.Single(registry.Contracts));

    var result = registry.Validate(new Dictionary<string, string?>
    {
      ["APP_NAME"] = "config-contract",
    });

    Assert.True(result.IsValid);
    Assert.Empty(result.Diagnostics);
  }
}