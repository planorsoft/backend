using FluentValidation;

namespace Planor.Application.Finances.Commands.UpdateFinance;

public class UpdateFinanceCommandValidator : AbstractValidator<UpdateFinanceCommand>
{
    public UpdateFinanceCommandValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(1024);
        
        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(0);
    }
}