using FluentValidation;

namespace Planor.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator: AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(4096);

        RuleFor(v => v.Price)
            .GreaterThanOrEqualTo(0);
    }
}