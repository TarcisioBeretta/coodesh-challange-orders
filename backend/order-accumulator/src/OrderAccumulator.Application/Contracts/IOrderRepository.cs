using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.Contracts;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<IEnumerable<Order>> GetBySymbolAsync(Symbol symbol, CancellationToken cancellationToken);
}
