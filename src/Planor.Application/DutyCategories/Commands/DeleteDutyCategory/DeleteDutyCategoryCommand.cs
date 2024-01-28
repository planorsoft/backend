using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.DutyCategories.Commands.DeleteDutyCategory;

public class DeleteDutyCategoryCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteDutyCategoryCommandHandler : IRequestHandler<DeleteDutyCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDutyCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDutyCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DutyCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(DutyCategory), request.Id.ToString());

        _context.DutyCategories.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}