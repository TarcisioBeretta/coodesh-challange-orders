using Microsoft.Extensions.DependencyInjection;
using OrderAccumulator.Infrastructure.Fix;

namespace OrderAccumulator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<FixSessionManager>();

        services.AddHostedService<FixHostedService>();

        return services;
    }
}