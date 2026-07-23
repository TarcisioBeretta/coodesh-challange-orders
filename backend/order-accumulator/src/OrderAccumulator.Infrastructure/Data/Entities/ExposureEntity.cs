using OrderAccumulator.Domain.Models;

namespace OrderAccumulator.Infrastructure.Data.Entities;

public class ExposureEntity
{
    public string Symbol { get; private set; }
    public decimal CurrentExposure { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public static ExposureEntity FromDomain(Exposure domain)
    {
        return new ExposureEntity
        {
            Symbol = domain.Symbol.Value,
            CurrentExposure = domain.CurrentExposure,
            LastUpdated = domain.LastUpdated
        };
    }

    public Exposure ToDomain()
    {
        return Exposure.Reconstruct(Domain.ValueObjects.Symbol.Reconstruct(Symbol), CurrentExposure, LastUpdated);
    }
}
