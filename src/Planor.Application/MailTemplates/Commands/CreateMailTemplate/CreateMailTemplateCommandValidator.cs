using FluentValidation;

namespace Planor.Application.MailTemplates.Commands.CreateMailTemplate;

public class CreateMailTemplateCommandValidator : AbstractValidator<CreateMailTemplateCommand>
{
    public CreateMailTemplateCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(64)
            .NotEmpty();
        
        RuleFor(v => v.Slug)
            .MaximumLength(64)
            .NotEmpty();
        
        RuleFor(v => v.Body)
            .NotEmpty();
    }
}