using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.DTOs;

public sealed record ProcessOrderResultDto(
    ExecType ExecType,
    Symbol Symbol,
    Side Side,
    int Quantity,
    decimal Price,
    decimal CurrentExposure,
    string? RejectReason);
