using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Apps.Commands.DeleteApp;

public class DeleteAppCommand: IRequest
{
    public int Id { get; set; }
}

public class DeleteAppCommandHandler : IRequestHandler<DeleteAppCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAppCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAppCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Apps
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(App), request.Id.ToString());
        }

        _context.Apps.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}