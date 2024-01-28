using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.CreateTag;
using Planor.Domain.Entities;
using Planor.Domain.Events;

namespace Planor.Application.MailTemplates.Commands.CreateMailTemplate;

public class CreateMailTemplateCommand : IRequest<int>
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Body { get; set; } = null!;
}

public class CreateMailTemplateCommandHandler : IRequestHandler<CreateMailTemplateCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateMailTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = new MailTemplate
        {
            Title = request.Title,
            Slug = request.Slug,
            Body = request.Body
        };

        _context.MailTemplates.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}