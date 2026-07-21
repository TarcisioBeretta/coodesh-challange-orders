using Moq;
using OrderGenerator.Application.Commands;
using OrderGenerator.Application.Contracts;
using OrderGenerator.Application.DTOs;
using OrderGenerator.Application.Handlers;
using OrderGenerator.Domain.Models;
using OrderGenerator.Domain.ValueObjects;

namespace OrderGenerator.Application.Tests;

public class SendOrderCommandHandlerTests
{
    private readonly SendOrderCommandHandler _handler;
    private readonly Mock<IOrderAccumulatorClient> _orderAccumulatorClient;

    public SendOrderCommandHandlerTests()
    {
        _orderAccumulatorClient = new Mock<IOrderAccumulatorClient>();
        _handler = new SendOrderCommandHandler(_orderAccumulatorClient.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsExecutionReportFromClient()
    {
        var symbol = Symbol.Create("PETR4");
        var command = new SendOrderCommand(symbol, Side.Buy, 100, 25.50m);
        var expectedReport = new ExecutionReportDto(ExecType.New, symbol, Side.Buy, 100, 25.50m, "ok");

        _orderAccumulatorClient
            .Setup(x => x.SendAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedReport);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedReport.ExecType, result.ExecType);
        Assert.Equal(expectedReport.Symbol, result.Symbol);
        Assert.Equal(expectedReport.Side, result.Side);
        Assert.Equal(expectedReport.Quantity, result.Quantity);
        Assert.Equal(expectedReport.Price, result.Price);
        Assert.Equal(expectedReport.Text, result.Text);

        _orderAccumulatorClient.Verify(
            x => x.SendAsync(It.Is<Order>(o => o.Symbol == symbol && o.Side == Side.Buy && o.Quantity == 100 && o.Price == 25.50m), It.IsAny<CancellationToken>()),
            Times.Once());
    }
}
