using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Duties.Commands.DeleteDuty;

public class DeleteDutyCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteDutyCommandHandler : IRequestHandler<DeleteDutyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDutyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDutyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Duties
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(Duty), request.Id.ToString());

        _context.Duties.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}