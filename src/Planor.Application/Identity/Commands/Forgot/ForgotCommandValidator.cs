using FluentValidation;

namespace Planor.Application.Identity.Commands.Forgot;

public class ForgotCommandValidator : AbstractValidator<ForgotCommand>
{
    public ForgotCommandValidator()
    {

        RuleFor(v => v.Email)
            .MaximumLength(32)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Tenant)
            .NotEmpty();
    }
}