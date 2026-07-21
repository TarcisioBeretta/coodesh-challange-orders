using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Domain.Models;

public class Exposure
{
    private const decimal MaxExposureLimit = 100_000_000m;

    public Symbol Symbol { get; private init; }

    public decimal CurrentExposure { get; private set; }

    public DateTime LastUpdated { get; private set; }

    public static Exposure Create(Symbol symbol)
    {
        return new Exposure(symbol, 0, DateTime.UtcNow);
    }

    public static Exposure Reconstruct(Symbol symbol, decimal currentExposure, DateTime lastUpdated)
    {
        return new Exposure(symbol, currentExposure, lastUpdated);
    }

    public void AddOrder(Order order)
    {
        if (order.ExecType != ExecType.New)
        {
            return;
        }

        if (order.Side == Side.Buy)
        {
            if (CurrentExposure + order.Value > MaxExposureLimit)
            {
                throw new Exceptions.DomainException(
                    $"Order would exceed exposure limit of R$ {MaxExposureLimit:N2} for symbol {Symbol}");
            }

            CurrentExposure += order.Value;
        }
        else
        {
            if (CurrentExposure - order.Value < -MaxExposureLimit)
            {
                throw new Domain.Exceptions.DomainException(
                    $"Order would exceed exposure limit of R$ {MaxExposureLimit:N2} for symbol {Symbol}");
            }

            CurrentExposure -= order.Value;
        }

        LastUpdated = DateTime.UtcNow;
    }

    private Exposure(Symbol symbol, decimal currentExposure, DateTime lastUpdated)
    {
        Symbol = symbol;
        CurrentExposure = currentExposure;
        LastUpdated = lastUpdated;
    }
}
