using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.Contracts;

public interface IExposureRepository
{
    Task AddAsync(Exposure exposure, CancellationToken cancellationToken);
    Task UpdateAsync(Exposure exposure, CancellationToken cancellationToken);
    Task<Exposure?> GetBySymbolAsync(Symbol symbol, CancellationToken cancellationToken);
}
