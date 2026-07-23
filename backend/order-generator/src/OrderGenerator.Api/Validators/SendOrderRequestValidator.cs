using FluentValidation;
using OrderGenerator.Api.DTOs;
using OrderGenerator.Domain.ValueObjects;

namespace OrderGenerator.Api.Validators;

public class SendOrderRequestValidator : AbstractValidator<SendOrderRequest>
{
    public SendOrderRequestValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty()
            .Custom((symbol, context) => {
                try
                {
                    Symbol.Create(symbol);
                }
                catch (ArgumentException ex)
                {
                    context.AddFailure(ex.Message);
                }
            });

        RuleFor(x => x.Side)
            .IsInEnum();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}
