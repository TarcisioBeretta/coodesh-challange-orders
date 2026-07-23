using Microsoft.Extensions.DependencyInjection;
using OrderAccumulator.Application.Contracts;
using OrderAccumulator.Application.Services;

namespace OrderAccumulator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddScoped<IOrderProcessor, OrderProcessor>();

        return services;
    }
}