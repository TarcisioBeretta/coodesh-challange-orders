using MediatR;
using OrderGenerator.Application.DTOs;
using OrderGenerator.Domain.Models;
using OrderGenerator.Domain.ValueObjects;

namespace OrderGenerator.Application.Commands;

public sealed record SendOrderCommand(
    Symbol Symbol,
    Side Side,
    int Quantity,
    decimal Price) : IRequest<ExecutionReportDto>;
