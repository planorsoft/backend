using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.DutyCategories.Commands.UpdateDutyCategory;

public class UpdateDutyCategoryCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
}

public class UpdateDutyCategoryCommandHandler : IRequestHandler<UpdateDutyCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDutyCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDutyCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DutyCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(DutyCategory), request.Id.ToString());

        entity.Title = request.Title;

        _context.DutyCategories.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}