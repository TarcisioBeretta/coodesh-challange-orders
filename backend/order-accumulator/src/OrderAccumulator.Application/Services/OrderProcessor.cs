using MediatR;
using OrderAccumulator.Application.Commands;
using OrderAccumulator.Application.Contracts;
using OrderAccumulator.Application.DTOs;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.Services;

public class OrderProcessor(IMediator mediator) : IOrderProcessor
{
    public async Task<ProcessOrderResultDto> ProcessAsync(
        Symbol symbol,
        Side side,
        int quantity,
        decimal price,
        CancellationToken cancellationToken)
    {
        var command = new ProcessOrderCommand(
            symbol,
            side,
            quantity,
            price);

        return await mediator.Send(command, cancellationToken);
    }
}