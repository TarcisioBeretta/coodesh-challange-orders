using Microsoft.Extensions.Hosting;

namespace OrderAccumulator.Infrastructure.Fix;

public class FixHostedService(FixSessionManager sessionManager) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        sessionManager.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        sessionManager.Stop();
        return Task.CompletedTask;
    }
}