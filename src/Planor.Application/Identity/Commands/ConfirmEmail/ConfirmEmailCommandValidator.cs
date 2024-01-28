using FluentValidation;

namespace Planor.Application.Identity.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {

        RuleFor(v => v.Email)
            .MaximumLength(32)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(v => v.Token)
            .Length(6)
            .NotEmpty();
    }
}