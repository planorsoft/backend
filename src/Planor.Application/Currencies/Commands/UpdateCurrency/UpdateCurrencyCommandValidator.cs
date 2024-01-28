using FluentValidation;
using Planor.Application.Currencies.Commands.CreateCurrency;

namespace Planor.Application.Currencies.Commands.UpdateCurrency;

public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
{
    public UpdateCurrencyCommandValidator()
    {
        RuleFor(v => v.Code)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(v => v.Symbol)
            .MaximumLength(64);
        
        RuleFor(v => v.Rate)
            .NotEmpty();
    }
}