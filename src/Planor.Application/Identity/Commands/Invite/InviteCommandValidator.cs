using FluentValidation;

namespace Planor.Application.Identity.Commands.Invite;

public class InviteCommandValidator : AbstractValidator<InviteCommand>
{
    public InviteCommandValidator()
    {

        RuleForEach(v => v.Emails)
            .MaximumLength(32)
            .EmailAddress();

    }
}