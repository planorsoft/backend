using FluentValidation;
using Planor.Application.Identity.Commands.Register;
using Planor.Domain.Constants;

namespace Planor.Application.Identity.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {

        RuleFor(v => v.Email)
            .MaximumLength(32)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(v => v.Password)
            .MaximumLength(64)
            .NotEmpty();
    }
}