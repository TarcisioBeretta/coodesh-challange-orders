using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.Exceptions;
using OrderAccumulator.Domain.ValueObjects;
using Xunit;

namespace OrderAccumulator.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void Create_ValidOrder_ReturnsOrder()
    {
        var symbol = Symbol.Create("PETR4");
        var order = Order.Create(symbol, Side.Buy, 100, 25.50m, ExecType.New);
        
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(symbol, order.Symbol);
        Assert.Equal(Side.Buy, order.Side);
        Assert.Equal(100, order.Quantity);
        Assert.Equal(25.50m, order.Price);
        Assert.Equal(ExecType.New, order.ExecType);
        Assert.True(order.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Create_QuantityZero_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 0, 25.50m, ExecType.New));
    }

    [Fact]
    public void Create_QuantityNegative_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, -100, 25.50m, ExecType.New));
    }

    [Fact]
    public void Create_QuantityEqualToLimit_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 100000, 25.50m, ExecType.New));
    }

    [Fact]
    public void Create_QuantityAboveLimit_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 100001, 25.50m, ExecType.New));
    }

    [Fact]
    public void Create_PriceZero_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 100, 0, ExecType.New));
    }

    [Fact]
    public void Create_PriceNegative_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 100, -25.50m, ExecType.New));
    }

    [Fact]
    public void Create_PriceEqualToLimit_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 100, 1000, ExecType.New));
    }

    [Fact]
    public void Create_PriceAboveLimit_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 100, 1000.01m, ExecType.New));
    }

    [Fact]
    public void Create_PriceNotMultipleOf001_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Throws<DomainException>(() => Order.Create(symbol, Side.Buy, 100, 25.501m, ExecType.New));
    }

    [Fact]
    public void GetOrderValue_BuyOrder_ReturnsPositiveValue()
    {
        var symbol = Symbol.Create("PETR4");
        var order = Order.Create(symbol, Side.Buy, 100, 25.50m, ExecType.New);
        
        Assert.Equal(2550m, order.Value);
    }

    [Fact]
    public void GetOrderValue_SellOrder_ReturnsPositiveValue()
    {
        var symbol = Symbol.Create("PETR4");
        var order = Order.Create(symbol, Side.Sell, 100, 25.50m, ExecType.New);
        
        Assert.Equal(2550m, order.Value);
    }

    [Fact]
    public void Reconstruct_ValidOrder_ReturnsOrder()
    {
        var symbol = Symbol.Create("PETR4");
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-1);
        
        var order = Order.Reconstruct(id, symbol, Side.Buy, 100, 25.50m, ExecType.New, createdAt);
        
        Assert.Equal(id, order.Id);
        Assert.Equal(symbol, order.Symbol);
        Assert.Equal(Side.Buy, order.Side);
        Assert.Equal(100, order.Quantity);
        Assert.Equal(25.50m, order.Price);
        Assert.Equal(ExecType.New, order.ExecType);
        Assert.Equal(createdAt, order.CreatedAt);
    }

    [Fact]
    public void Reconstruct_DoesNotValidate_AcceptsInvalidValues()
    {
        var symbol = Symbol.Reconstruct("INVALID");
        var id = Guid.NewGuid();
        
        var order = Order.Reconstruct(id, symbol, Side.Buy, 100000, 1000, ExecType.New, DateTime.UtcNow);
        
        Assert.Equal(100000, order.Quantity);
        Assert.Equal(1000, order.Price);
    }
}
