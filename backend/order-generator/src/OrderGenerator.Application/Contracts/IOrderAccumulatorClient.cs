using OrderGenerator.Application.DTOs;
using OrderGenerator.Domain.Models;

namespace OrderGenerator.Application.Contracts;

public interface IOrderAccumulatorClient
{
    Task<ExecutionReportDto> SendAsync(Order order, CancellationToken cancellationToken);
}
