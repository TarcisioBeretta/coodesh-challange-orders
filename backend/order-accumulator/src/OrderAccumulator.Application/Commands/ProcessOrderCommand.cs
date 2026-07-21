using MediatR;
using OrderAccumulator.Application.DTOs;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.Commands;

public sealed record ProcessOrderCommand(
    Symbol Symbol,
    Side Side,
    int Quantity,
    decimal Price) : IRequest<ProcessOrderResultDto>;
