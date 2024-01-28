using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Tags.Commands.DeleteTag;

public class DeleteTagCommand: IRequest
{
    public int Id { get; set; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Tag), request.Id.ToString());
        }

        _context.Tags.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}