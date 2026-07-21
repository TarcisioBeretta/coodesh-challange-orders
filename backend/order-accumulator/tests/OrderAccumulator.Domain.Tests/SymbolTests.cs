using OrderAccumulator.Domain.ValueObjects;
using Xunit;

namespace OrderAccumulator.Domain.Tests;

public class SymbolTests
{
    [Fact]
    public void Create_ValidSymbol_ReturnsSymbol()
    {
        var symbol = Symbol.Create("PETR4");
        
        Assert.Equal("PETR4", symbol.Value);
    }

    [Fact]
    public void Create_ValidSymbolUpperCase_ReturnsSymbol()
    {
        var symbol = Symbol.Create("petr4");
        
        Assert.Equal("PETR4", symbol.Value);
    }

    [Theory]
    [InlineData("PETR4")]
    [InlineData("VALE3")]
    [InlineData("VIIA4")]
    public void Create_AllValidSymbols_ReturnsSymbol(string symbolValue)
    {
        var symbol = Symbol.Create(symbolValue);
        
        Assert.Equal(symbolValue, symbol.Value);
    }

    [Fact]
    public void Create_EmptySymbol_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Symbol.Create(""));
    }

    [Fact]
    public void Create_InvalidSymbol_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Symbol.Create("INVALID"));
    }

    [Fact]
    public void Reconstruct_ValidSymbol_ReturnsSymbol()
    {
        var symbol = Symbol.Reconstruct("PETR4");
        
        Assert.Equal("PETR4", symbol.Value);
    }

    [Fact]
    public void Reconstruct_DoesNotValidate_AcceptsAnyValue()
    {
        var symbol = Symbol.Reconstruct("INVALID");
        
        Assert.Equal("INVALID", symbol.Value);
    }

    [Fact]
    public void Equals_DifferentSymbol_ReturnsFalse()
    {
        var symbol1 = Symbol.Create("PETR4");
        var symbol2 = Symbol.Create("VALE3");
        
        Assert.NotEqual(symbol1, symbol2);
    }
}
