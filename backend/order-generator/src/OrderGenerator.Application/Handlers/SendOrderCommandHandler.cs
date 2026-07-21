using MediatR;
using OrderGenerator.Application.Commands;
using OrderGenerator.Application.Contracts;
using OrderGenerator.Application.DTOs;
using OrderGenerator.Domain.Models;
using OrderGenerator.Domain.ValueObjects;

namespace OrderGenerator.Application.Handlers;

public class SendOrderCommandHandler(
    IOrderAccumulatorClient _orderAccumulatorClient) : IRequestHandler<SendOrderCommand, ExecutionReportDto>
{
    public async Task<ExecutionReportDto> Handle(SendOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.Symbol, request.Side, request.Quantity, request.Price);

        return await _orderAccumulatorClient.SendAsync(order, cancellationToken);
    }
}
