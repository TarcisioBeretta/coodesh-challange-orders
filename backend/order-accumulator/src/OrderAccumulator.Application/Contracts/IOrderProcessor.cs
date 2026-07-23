using OrderAccumulator.Application.DTOs;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.Contracts;

public interface IOrderProcessor
{
    Task<ProcessOrderResultDto> ProcessAsync(
        Symbol symbol,
        Side side,
        int quantity,
        decimal price,
        CancellationToken cancellationToken);
}