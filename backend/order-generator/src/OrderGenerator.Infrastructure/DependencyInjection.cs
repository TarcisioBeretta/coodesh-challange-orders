using Microsoft.Extensions.DependencyInjection;
using OrderGenerator.Application.Contracts;
using OrderGenerator.Infrastructure.Fix;


namespace OrderGenerator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<FixApplication>();
        services.AddSingleton<FixSessionManager>();
        services.AddSingleton<PendingExecutionReportStore>();
        services.AddSingleton<IOrderAccumulatorClient, FixOrderAccumulatorClient>();

        services.AddHostedService<FixHostedService>();

        return services;
    }
}