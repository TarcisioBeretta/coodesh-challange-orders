using OrderAccumulator.Application.Contracts;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;
using OrderAccumulator.Infrastructure.Data.Entities;

namespace OrderAccumulator.Infrastructure.Data.Repositories
{
    public class ExposureRepository(AppDbContext context) : IExposureRepository
    {
        public async Task AddAsync(Exposure exposure, CancellationToken cancellationToken)
        {
            var exposureEntity = ExposureEntity.FromDomain(exposure);
            await context.Exposures.AddAsync(exposureEntity, cancellationToken);
        }

        public async Task<Exposure?> GetBySymbolAsync(Symbol symbol, CancellationToken cancellationToken)
        {
            var exposureEntity = await context.Exposures.FindAsync([symbol.Value], cancellationToken);
            return exposureEntity?.ToDomain();
        }

        public Task UpdateAsync(Exposure exposure, CancellationToken cancellationToken)
        {
            var exposureEntity = ExposureEntity.FromDomain(exposure);
            context.Exposures.Update(exposureEntity);
            return Task.CompletedTask;
        }
    }
}