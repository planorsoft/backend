using FluentValidation;

namespace Planor.Application.DutySizes.Commands.CreateDutySize;

public class CreateDutySizeCommandValidator : AbstractValidator<CreateDutySizeCommand>
{
    public CreateDutySizeCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(v => v.Score)
            .GreaterThanOrEqualTo(0);
    }
}