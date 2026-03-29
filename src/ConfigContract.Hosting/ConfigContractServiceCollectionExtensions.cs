using Microsoft.Extensions.DependencyInjection;

namespace ConfigContract.Hosting;

public static class ConfigContractServiceCollectionExtensions
{
  public static IServiceCollection AddConfigContract(this IServiceCollection services)
  {
    ArgumentNullException.ThrowIfNull(services);

    services.AddSingleton<ContractRegistry>();
    return services;
  }
}