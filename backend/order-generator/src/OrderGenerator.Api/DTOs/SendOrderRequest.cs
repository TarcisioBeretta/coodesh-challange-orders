using OrderGenerator.Domain.Models;

namespace OrderGenerator.Api.DTOs;

public sealed record SendOrderRequest(
    string Symbol,
    Side Side,
    int Quantity,
    decimal Price);
