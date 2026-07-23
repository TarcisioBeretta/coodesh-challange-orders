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

        public async Task UpdateAsync(Exposure exposure, CancellationToken cancellationToken)
        {
            var exposureEntity = await context.Exposures.FindAsync([exposure.Symbol.Value], cancellationToken);
            var updatedEntity = ExposureEntity.FromDomain(exposure);

            if (exposureEntity is null)
            {
                throw new InvalidOperationException("Exposure not found.");
            }

            context.Entry(exposureEntity).CurrentValues.SetValues(updatedEntity);
        }
    }
}