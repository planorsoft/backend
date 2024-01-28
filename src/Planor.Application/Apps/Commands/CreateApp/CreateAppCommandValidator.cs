using FluentValidation;

namespace Planor.Application.Apps.Commands.CreateApp;


public class CreateAppCommandValidator : AbstractValidator<CreateAppCommand>
{
    public CreateAppCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(64)
            .NotEmpty();
    }
}