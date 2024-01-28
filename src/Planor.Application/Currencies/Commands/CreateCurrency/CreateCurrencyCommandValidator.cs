using FluentValidation;
using Planor.Application.MailTemplates.Commands.CreateMailTemplate;

namespace Planor.Application.Currencies.Commands.CreateCurrency;

public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyCommandValidator()
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