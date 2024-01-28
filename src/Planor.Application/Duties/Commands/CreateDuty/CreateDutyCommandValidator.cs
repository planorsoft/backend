using FluentValidation;

namespace Planor.Application.Duties.Commands.CreateDuty;

public class CreateDutyCommandValidator : AbstractValidator<CreateDutyCommand>
{
    public CreateDutyCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(4096);

    }
}