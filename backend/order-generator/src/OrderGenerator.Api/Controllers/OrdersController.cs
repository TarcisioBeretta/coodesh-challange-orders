using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Api.DTOs;
using OrderGenerator.Application.Commands;
using OrderGenerator.Domain.ValueObjects;

namespace OrderGenerator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendOrder([FromBody] SendOrderRequest request)
    {
        var symbol = Symbol.Create(request.Symbol);
        var command = new SendOrderCommand(symbol, request.Side, request.Quantity, request.Price);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
