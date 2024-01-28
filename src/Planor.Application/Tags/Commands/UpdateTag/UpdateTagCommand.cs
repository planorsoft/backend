using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Tags.Commands.UpdateTag;

public class UpdateTagCommand : IRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Tag), request.Id.ToString());
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}