using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.DutySizes.Commands.DeleteDutySize;

public class DeleteDutySizeCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteDutySizeCommandHandler : IRequestHandler<DeleteDutySizeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDutySizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDutySizeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DutySizes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(DutySize), request.Id.ToString());

        _context.DutySizes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}