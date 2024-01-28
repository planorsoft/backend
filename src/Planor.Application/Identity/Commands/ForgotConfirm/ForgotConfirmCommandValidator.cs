using FluentValidation;

namespace Planor.Application.Identity.Commands.ForgotConfirm;

public class ForgotConfirmCommandValidator : AbstractValidator<ForgotConfirmCommand>
{
    public ForgotConfirmCommandValidator()
    {

        RuleFor(v => v.Email)
            .MaximumLength(32)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(v => v.Token)
            .NotEmpty();
        
        RuleFor(x => x.Tenant)
            .NotEmpty();
    }
}