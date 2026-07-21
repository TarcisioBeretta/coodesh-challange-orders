using OrderGenerator.Domain.Exceptions;
using OrderGenerator.Domain.ValueObjects;

namespace OrderGenerator.Domain.Models;

public class Order
{
    public Guid Id { get; private init; }

    public Symbol Symbol { get; private init; }

    public Side Side { get; private init; }

    public int Quantity { get; private init; }

    public decimal Price { get; private init; }

    public DateTime CreatedAt { get; private init; }

    public static Order Create(
        Symbol symbol,
        Side side,
        int quantity,
        decimal price)
    {
        ValidateQuantity(quantity);
        ValidatePrice(price);

        return new Order(
            Guid.NewGuid(),
            symbol,
            side,
            quantity,
            price,
            DateTime.UtcNow);
    }

    public static Order Reconstruct(
        Guid id,
        Symbol symbol,
        Side side,
        int quantity,
        decimal price,
        DateTime createdAt)
    {
        return new Order(
            id,
            symbol,
            side,
            quantity,
            price,
            createdAt);
    }

    private Order(
        Guid id,
        Symbol symbol,
        Side side,
        int quantity,
        decimal price,
        DateTime createdAt)
    {
        Id = id;
        Symbol = symbol;
        Side = side;
        Quantity = quantity;
        Price = price;
        CreatedAt = createdAt;
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be positive");
        }

        if (quantity >= 100000)
        {
            throw new DomainException("Quantity must be less than 100,000");
        }
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
        {
            throw new DomainException("Price must be positive");
        }

        if (price >= 1000)
        {
            throw new DomainException("Price must be less than 1,000");
        }

        if (price % 0.01m != 0)
        {
            throw new DomainException("Price must be a multiple of 0.01");
        }
    }
}
