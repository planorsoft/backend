using FluentValidation;

namespace Planor.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.Email)
            .MaximumLength(64)
            .EmailAddress()
            .NotEmpty();

        RuleFor(v => v.Name)
            .MaximumLength(64);
        
        RuleFor(v => v.Username)
            .MaximumLength(64);
        
        RuleFor(v => v.OldPassword)
            .MaximumLength(64);
        
        RuleFor(v => v.NewPassword)
            .MaximumLength(64);
    }
}