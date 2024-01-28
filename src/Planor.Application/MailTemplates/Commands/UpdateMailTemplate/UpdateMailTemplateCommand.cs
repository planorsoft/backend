using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.UpdateTag;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.MailTemplates.Commands.UpdateMailTemplate;

public class UpdateMailTemplateCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Body { get; set; } = null!;
}

public class UpdateMailTemplateCommandHandler : IRequestHandler<UpdateMailTemplateCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMailTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MailTemplates
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(MailTemplate), request.Id.ToString());
        }

        entity.Title = request.Title;
        entity.Slug = request.Slug;
        entity.Body = request.Body;

        await _context.SaveChangesAsync(cancellationToken);
    }
}