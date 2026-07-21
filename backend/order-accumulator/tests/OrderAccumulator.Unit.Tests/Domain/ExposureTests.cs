using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.Exceptions;
using OrderAccumulator.Domain.ValueObjects;
using Xunit;

namespace OrderAccumulator.Domain.Tests;

public class ExposureTests
{
    [Fact]
    public void Create_ReturnsExposureWithZeroValue()
    {
        var symbol = Symbol.Create("PETR4");
        var exposure = Exposure.Create(symbol);
        
        Assert.Equal(symbol, exposure.Symbol);
        Assert.Equal(0, exposure.CurrentExposure);
        Assert.True(exposure.LastUpdated <= DateTime.UtcNow);
    }

    [Fact]
    public void Reconstruct_ReturnsExposureWithGivenValues()
    {
        var symbol = Symbol.Create("PETR4");
        var lastUpdated = DateTime.UtcNow.AddDays(-1);
        
        var exposure = Exposure.Reconstruct(symbol, 50000m, lastUpdated);
        
        Assert.Equal(symbol, exposure.Symbol);
        Assert.Equal(50000m, exposure.CurrentExposure);
        Assert.Equal(lastUpdated, exposure.LastUpdated);
    }

    [Fact]
    public void AddOrder_BuyOrder_IncreasesExposure()
    {
        var symbol = Symbol.Create("PETR4");
        var exposure = Exposure.Create(symbol);
        var order = Order.Create(symbol, Side.Buy, 100, 25.50m, ExecType.New);
        
        exposure.AddOrder(order);
        
        Assert.Equal(2550m, exposure.CurrentExposure);
    }

    [Fact]
    public void AddOrder_SellOrder_DecreasesExposure()
    {
        var symbol = Symbol.Create("PETR4");
        var exposure = Exposure.Create(symbol);
        var buyOrder = Order.Create(symbol, Side.Buy, 100, 25.50m, ExecType.New);
        exposure.AddOrder(buyOrder);
        
        var sellOrder = Order.Create(symbol, Side.Sell, 50, 25.50m, ExecType.New);
        exposure.AddOrder(sellOrder);
        
        Assert.Equal(1275m, exposure.CurrentExposure);
    }

    [Fact]
    public void AddOrder_RejectedOrder_DoesNotChangeExposure()
    {
        var symbol = Symbol.Create("PETR4");
        var exposure = Exposure.Create(symbol);
        var order = Order.Create(symbol, Side.Buy, 100, 25.50m, ExecType.Rejected);
        
        exposure.AddOrder(order);
        
        Assert.Equal(0, exposure.CurrentExposure);
    }

    [Fact]
    public void AddOrder_BuyOrderExceedsLimit_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        var exposure = Exposure.Reconstruct(symbol, 99_999_000m, DateTime.UtcNow);
        var order = Order.Create(symbol, Side.Buy, 100, 25.50m, ExecType.New);
        
        Assert.Throws<DomainException>(() => exposure.AddOrder(order));
    }

    [Fact]
    public void AddOrder_SellOrderExceedsLimit_ThrowsDomainException()
    {
        var symbol = Symbol.Create("PETR4");
        var exposure = Exposure.Reconstruct(symbol, -99_999_000m, DateTime.UtcNow);
        var order = Order.Create(symbol, Side.Sell, 100, 25.50m, ExecType.New);
        
        Assert.Throws<DomainException>(() => exposure.AddOrder(order));
    }
}
