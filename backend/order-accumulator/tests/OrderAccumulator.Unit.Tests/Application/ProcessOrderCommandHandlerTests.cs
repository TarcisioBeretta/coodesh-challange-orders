using Moq;
using OrderAccumulator.Application.Commands;
using OrderAccumulator.Application.Contracts;
using OrderAccumulator.Application.DTOs;
using OrderAccumulator.Application.Handlers;
using OrderAccumulator.Domain.Exceptions;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.Tests;

public class ProcessOrderCommandHandlerTests
{
    private readonly ProcessOrderCommandHandler _handler;
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IExposureRepository> _exposureRepository;

    public ProcessOrderCommandHandlerTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _exposureRepository = new Mock<IExposureRepository>();
        _handler = new ProcessOrderCommandHandler(_orderRepository.Object, _exposureRepository.Object);
    }

    [Fact]
    public async Task Handle_NewOrderWithNoExistingExposure_AddsOrderAndExposure()
    {
        var symbol = Symbol.Create("PETR4");
        var command = new ProcessOrderCommand(symbol, Side.Buy, 100, 25.50m);

        _exposureRepository
            .Setup(x => x.GetBySymbolAsync(symbol, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Exposure?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        AssertResult(result, ExecType.New, symbol, Side.Buy, 100, 25.50m, 2550m, null);

        VerifyOrderAdded(ExecType.New);
        VerifyExposureAdded(symbol);
        VerifyExposureUpdated(Times.Never());
    }

    [Fact]
    public async Task Handle_NewOrderWithExistingExposure_UpdatesExposure()
    {
        var symbol = Symbol.Create("PETR4");
        var existingExposure = Exposure.Create(symbol);
        var command = new ProcessOrderCommand(symbol, Side.Buy, 100, 25.50m);

        _exposureRepository
            .SetupSequence(x => x.GetBySymbolAsync(symbol, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExposure)
            .ReturnsAsync(existingExposure);

        var result = await _handler.Handle(command, CancellationToken.None);

        AssertResult(result, ExecType.New, symbol, Side.Buy, 100, 25.50m, 2550m, null);

        VerifyOrderAdded(ExecType.New);
        VerifyExposureAdded(Times.Never());
        VerifyExposureUpdated(symbol, Times.Once());
    }

    [Fact]
    public async Task Handle_OrderExceedingLimit_ReturnsRejectedResult()
    {
        var symbol = Symbol.Create("PETR4");
        var existingExposure = Exposure.Reconstruct(symbol, 99_999_000m, DateTime.UtcNow);
        var command = new ProcessOrderCommand(symbol, Side.Buy, 100, 25.50m);

        _exposureRepository
            .SetupSequence(x => x.GetBySymbolAsync(symbol, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExposure)
            .ReturnsAsync(existingExposure);

        var result = await _handler.Handle(command, CancellationToken.None);

        AssertResult(result, ExecType.Rejected, symbol, Side.Buy, 100, 25.50m, 99_999_000m, result.RejectReason);

        VerifyOrderAdded(ExecType.Rejected);
        VerifyExposureAdded(Times.Never());
        VerifyExposureUpdated(Times.Never());
    }

    private static void AssertResult(
        ProcessOrderResultDto result,
        ExecType expectedExecType,
        Symbol expectedSymbol,
        Side expectedSide,
        int expectedQuantity,
        decimal expectedPrice,
        decimal expectedCurrentExposure,
        string? expectedRejectReason)
    {
        Assert.Equal(expectedExecType, result.ExecType);
        Assert.Equal(expectedSymbol, result.Symbol);
        Assert.Equal(expectedSide, result.Side);
        Assert.Equal(expectedQuantity, result.Quantity);
        Assert.Equal(expectedPrice, result.Price);
        Assert.Equal(expectedCurrentExposure, result.CurrentExposure);
        Assert.Equal(expectedRejectReason, result.RejectReason);
    }

    private void VerifyOrderAdded(ExecType execType, Times? times = null)
    {
        _orderRepository.Verify(
            x => x.AddAsync(It.Is<Order>(o => o.ExecType == execType), It.IsAny<CancellationToken>()),
            times ?? Times.Once());
    }

    private void VerifyExposureAdded(Symbol symbol, Times? times = null)
    {
        _exposureRepository.Verify(
            x => x.AddAsync(It.Is<Exposure>(e => e.Symbol == symbol), It.IsAny<CancellationToken>()),
            times ?? Times.Once());
    }

    private void VerifyExposureAdded(Times times)
    {
        _exposureRepository.Verify(
            x => x.AddAsync(It.IsAny<Exposure>(), It.IsAny<CancellationToken>()),
            times);
    }

    private void VerifyExposureUpdated(Symbol symbol, Times? times = null)
    {
        _exposureRepository.Verify(
            x => x.UpdateAsync(It.Is<Exposure>(e => e.Symbol == symbol), It.IsAny<CancellationToken>()),
            times ?? Times.Once());
    }

    private void VerifyExposureUpdated(Times times)
    {
        _exposureRepository.Verify(
            x => x.UpdateAsync(It.IsAny<Exposure>(), It.IsAny<CancellationToken>()),
            times);
    }
}
