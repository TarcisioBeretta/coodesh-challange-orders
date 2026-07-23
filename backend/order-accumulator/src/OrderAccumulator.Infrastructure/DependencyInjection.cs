using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderAccumulator.Application.Contracts;
using OrderAccumulator.Infrastructure.Data;
using OrderAccumulator.Infrastructure.Data.Repositories;
using OrderAccumulator.Infrastructure.Fix;

namespace OrderAccumulator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("OrderAccumulator"));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IExposureRepository, ExposureRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<FixApplication>();
        services.AddSingleton<FixSessionManager>();

        services.AddHostedService<FixHostedService>();

        return services;
    }
}