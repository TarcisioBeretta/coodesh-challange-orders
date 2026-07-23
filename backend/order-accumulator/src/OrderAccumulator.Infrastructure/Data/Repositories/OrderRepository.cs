using Microsoft.EntityFrameworkCore;
using OrderAccumulator.Application.Contracts;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;
using OrderAccumulator.Infrastructure.Data.Entities;

namespace OrderAccumulator.Infrastructure.Data.Repositories
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            var orderEntity = OrderEntity.FromDomain(order);
            await context.Orders.AddAsync(orderEntity, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetBySymbolAsync(Symbol symbol, CancellationToken cancellationToken)
        {
            var orderEntities = await context.Orders
                .Where(o => o.Symbol == symbol.Value)
                .ToListAsync(cancellationToken);

            return orderEntities.Select(o => o.ToDomain());
        }
    }
}