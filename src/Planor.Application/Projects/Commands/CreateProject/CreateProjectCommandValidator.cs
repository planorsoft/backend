using System.Data;
using FluentValidation;
using Planor.Application.Tags.Commands.CreateTag;

namespace Planor.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
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