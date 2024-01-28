using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Duties.Commands.UpdateDuty;

public class UpdateDutyCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int? SizeId { get; set; }
    public int? PriorityId { get; set; }
    public int? CategoryId { get; set; }
    public int? ProjectId { get; set; }
    public int Order { get; set; }
}

public class UpdateDutyCommandHandler : IRequestHandler<UpdateDutyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateDutyCommandHandler(
        IApplicationDbContext context, 
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateDutyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Duties
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(Duty), request.Id.ToString());

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Order = request.Order;
        entity.AssignedTo = _currentUserService.Email ?? String.Empty;
        
        if (request.SizeId.HasValue)
        {
            var size = await _context.DutySizes.FirstOrDefaultAsync(x => x.Id == request.SizeId, cancellationToken);
            if (size is null) throw new NotFoundException(nameof(DutySize), request.SizeId);
            entity.Size = size;
        }

        if (request.PriorityId.HasValue)
        {
            var priority = await _context.DutyPriorities.FirstOrDefaultAsync(x => x.Id == request.PriorityId, cancellationToken);
            if (priority is null) throw new NotFoundException(nameof(DutyPriority), request.PriorityId);
            entity.Priority = priority;
        }

        if (request.CategoryId.HasValue)
        {
            var category = await _context.DutyCategories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
            if (category is null) throw new NotFoundException(nameof(DutyCategory), request.CategoryId);
            entity.Category = category;
        }

        if (request.ProjectId.HasValue)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);
            if (project is null) throw new NotFoundException(nameof(Project), request.ProjectId);
            entity.Project = project;
        }

        _context.Duties.Update(entity);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}