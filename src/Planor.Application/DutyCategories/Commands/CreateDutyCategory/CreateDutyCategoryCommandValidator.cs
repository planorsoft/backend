using FluentValidation;

namespace Planor.Application.DutyCategories.Commands.CreateDutyCategory;

public class CreateDutyCategoryCommandValidator : AbstractValidator<CreateDutyCategoryCommand>
{
    public CreateDutyCategoryCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(64)
            .NotEmpty();
    }
}