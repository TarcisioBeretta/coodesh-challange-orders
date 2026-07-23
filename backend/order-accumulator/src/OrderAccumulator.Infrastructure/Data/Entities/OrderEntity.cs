using OrderAccumulator.Domain.Models;

namespace OrderAccumulator.Infrastructure.Data.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public Side Side { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ExecType ExecType { get; set; }
    public DateTime CreatedAt { get; set; }

    public static OrderEntity FromDomain(Order domain)
    {
        return new OrderEntity
        {
            Id = domain.Id,
            Symbol = domain.Symbol.Value,
            Side = domain.Side,
            Quantity = domain.Quantity,
            Price = domain.Price,
            ExecType = domain.ExecType,
            CreatedAt = domain.CreatedAt
        };
    }

    public Order ToDomain()
    {
        return Order.Reconstruct(Id, Domain.ValueObjects.Symbol.Reconstruct(Symbol), Side, Quantity, Price, ExecType, CreatedAt);
    }
}
