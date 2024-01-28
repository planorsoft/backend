using FluentValidation;
using Planor.Domain.Entities;

namespace Planor.Application.Tags.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();

        var slugs = new List<string>() { nameof(User) };
        RuleFor(x => x.Slug)
            .NotEmpty()
            .Must(x => slugs.Contains(x));
    }
}