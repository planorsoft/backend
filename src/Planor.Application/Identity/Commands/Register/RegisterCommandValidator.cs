using FluentValidation;
using Planor.Domain.Constants;

namespace Planor.Application.Identity.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(32)
            .NotEmpty();
        
        RuleFor(v => v.Username)
            .MaximumLength(32)
            .NotEmpty();
        
        RuleFor(v => v.Email)
            .MaximumLength(32)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(v => v.Password)
            .MaximumLength(64)
            .NotEmpty();
        
        RuleFor(v => v.Role)
            .Must(x => Domain.Constants.Roles.List.Contains(x))
            .NotEmpty();
        
        RuleFor(v => v.Tenant)
            .MaximumLength(32)
            .Matches(@"^[a-z0-9]+(-[a-z0-9]+)*$")
            .NotEmpty();
    }
}