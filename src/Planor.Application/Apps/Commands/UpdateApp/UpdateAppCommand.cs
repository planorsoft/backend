using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Apps.Commands.UpdateApp;


public class UpdateAppCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class UpdateAppCommandHandler : IRequestHandler<UpdateAppCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAppCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAppCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Apps
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(App), request.Id.ToString());
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}