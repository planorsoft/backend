using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.DeleteTag;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.MailTemplates.Commands.DeleteMailTemplate;

public class DeleteMailTemplateCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteMailTemplateCommandHandler : IRequestHandler<DeleteMailTemplateCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMailTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MailTemplates
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(MailTemplate), request.Id.ToString());
        }

        _context.MailTemplates.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}