using FluentValidation;
using Planor.Application.Duties.Commands.CreateDuty;

namespace Planor.Application.Duties.Commands.UpdateDuty;

public class UpdateDutyCommandValidator : AbstractValidator<UpdateDutyCommand>
{
    public UpdateDutyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0)
            .NotEmpty();
        
        RuleFor(v => v.Title)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(4096);

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0);
    }
}