using FluentValidation;
using Planor.Application.Identity.Commands.Register;

namespace Planor.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(32)
            .NotEmpty();
        
        RuleFor(v => v.Email)
            .MaximumLength(32)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(v => v.Password)
            .MaximumLength(64)
            .NotEmpty();
    }
}