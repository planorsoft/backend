using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.DutySizes.Commands.UpdateDutySize;

public class UpdateDutySizeCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Score { get; set; }
}

public class UpdateDutySizeCommandHandler : IRequestHandler<UpdateDutySizeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDutySizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDutySizeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DutySizes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(DutySize), request.Id.ToString());

        entity.Name = request.Name;
        entity.Score = request.Score;

        _context.DutySizes.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}