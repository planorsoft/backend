using FluentValidation;

namespace Planor.Application.Apps.Commands.UpdateApp;


public class UpdateAppCommandValidator : AbstractValidator<UpdateAppCommand>
{
    public UpdateAppCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(64)
            .NotEmpty();
    }
}