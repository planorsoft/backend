using FluentValidation;

namespace Planor.Application.Finances.Commands.CreateFinance;

public class CreateFinanceCommandValidator : AbstractValidator<CreateFinanceCommand>
{
    public CreateFinanceCommandValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(1024);

        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(0);
    }
}