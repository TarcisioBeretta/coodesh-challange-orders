using OrderGenerator.Domain.Models;
using OrderGenerator.Domain.ValueObjects;

namespace OrderGenerator.Application.DTOs;

public sealed record ExecutionReportDto(
    ExecType ExecType,
    Symbol Symbol,
    Side Side,
    int Quantity,
    decimal Price,
    string? Text);
