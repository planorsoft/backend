using FluentValidation;

namespace Planor.Application.Roles.Commands.AddToRole;

public class AddToRoleCommandValidator : AbstractValidator<AddToRoleCommand>
{
    public AddToRoleCommandValidator()
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