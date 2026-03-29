using ConfigContract.Abstractions;

namespace ConfigContract.Generation;

public interface IContractCodeGenerator
{
  string ContractName { get; }

  string Generate(ContractDescriptor descriptor);
}