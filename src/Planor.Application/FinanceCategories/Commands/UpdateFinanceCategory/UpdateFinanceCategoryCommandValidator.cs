using FluentValidation;

namespace Planor.Application.FinanceCategories.Commands.UpdateFinanceCategory;

public class UpdateFinanceCategoryCommandValidator : AbstractValidator<UpdateFinanceCategoryCommand>
{
    public UpdateFinanceCategoryCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(255)
            .NotEmpty();
    }
}