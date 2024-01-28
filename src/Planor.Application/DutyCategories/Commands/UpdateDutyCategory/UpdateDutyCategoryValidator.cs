using FluentValidation;

namespace Planor.Application.DutyCategories.Commands.UpdateDutyCategory;

public class UpdateDutyCategoryCommandValidator : AbstractValidator<UpdateDutyCategoryCommand>
{
    public UpdateDutyCategoryCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(64)
            .NotEmpty();
    }
}