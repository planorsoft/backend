using FluentValidation;

namespace Planor.Application.FinanceCategories.Commands.CreateFinanceCategory;

public class CreateFinanceCategoryCommandValidator : AbstractValidator<CreateFinanceCategoryCommand>
{
    public CreateFinanceCategoryCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(255)
            .NotEmpty();
    }
}