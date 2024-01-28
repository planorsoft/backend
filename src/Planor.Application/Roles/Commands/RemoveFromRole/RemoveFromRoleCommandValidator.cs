using FluentValidation;

namespace Planor.Application.Roles.Commands.RemoveFromRole;

public class RemoveFromRoleCommandValidator : AbstractValidator<RemoveFromRoleCommand>
{
    public RemoveFromRoleCommandValidator()
    {

        RuleFor(v => v.Email)
            .MaximumLength(32)
            .EmailAddress()
            .NotEmpty();

        RuleFor(v => v.Role)
            .Must(x => Domain.Constants.Roles.ExtendedList.Contains(x))
            .NotEmpty();
    }
}