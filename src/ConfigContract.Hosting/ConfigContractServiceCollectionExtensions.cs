using Microsoft.Extensions.DependencyInjection;

namespace ConfigContract.Hosting;

public static class ConfigContractServiceCollectionExtensions
{
  public static IServiceCollection AddConfigContract(this IServiceCollection services)
  {
    return AddConfigContract(services, static _ => { });
  }

  public static IServiceCollection AddConfigContract(this IServiceCollection services, Action<ContractRegistry> configure)
  {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(configure);

    services.AddSingleton(_ =>
    {
      var registry = new ContractRegistry();
      configure(registry);
      return registry;
    });

    return services;
  }
}