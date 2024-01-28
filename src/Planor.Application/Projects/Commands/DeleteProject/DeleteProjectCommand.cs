using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(Project), request.Id.ToString());

        _context.Projects.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}