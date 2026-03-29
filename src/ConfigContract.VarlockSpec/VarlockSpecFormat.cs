using ConfigContract.Abstractions;

namespace ConfigContract.VarlockSpec;

public static class VarlockSpecFormat
{
  public const string Name = "varlock";

  public static ContractDescriptor CreateDescriptor(string name)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);

    return new ContractDescriptor(name);
  }
}