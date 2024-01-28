using FluentValidation;

namespace Planor.Application.DutySizes.Commands.UpdateDutySize;

public class UpdateDutySizeCommandValidator : AbstractValidator<UpdateDutySizeCommand>
{
    public UpdateDutySizeCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(v => v.Score)
            .GreaterThanOrEqualTo(0);
    }
}