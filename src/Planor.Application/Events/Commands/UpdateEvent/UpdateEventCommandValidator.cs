using FluentValidation;
using Planor.Application.Events.Commands.CreateEvent;
using Planor.Domain.Constants;

namespace Planor.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(128)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(2048);
        
        RuleFor(x => x.Location)
            .MaximumLength(1024);

        RuleFor(v => v.Start)
            .NotEmpty();
        
        RuleFor(v => v.Color)
            .Must(x => Colors.List.Contains(x))
            .NotEmpty();
        
        RuleForEach(x => x.Attendee)
            .EmailAddress();
    }
}