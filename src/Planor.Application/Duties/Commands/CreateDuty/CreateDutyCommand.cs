using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Duties.Commands.CreateDuty;

public class CreateDutyCommand : IRequest<int>
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? SizeId { get; set; }

    public int? PriorityId { get; set; }

    public int CategoryId { get; set; }

    public int ProjectId { get; set; }

    public string? AssignedTo { get; set; }

    public ICollection<string> Helpers { get; set; } = new List<string>();
}

public class CreateDutyCommandHandler : IRequestHandler<CreateDutyCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public CreateDutyCommandHandler(
        IApplicationDbContext context,
        UserManager<User> userManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateDutyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Duty
        {
            Title = request.Title,
            Description = request.Description,
            AssignedTo = request.AssignedTo ?? String.Empty
        };

        if (request.SizeId.HasValue)
        {
            var size = await _context.DutySizes.FirstOrDefaultAsync(
                x => x.Id == request.SizeId, 
                cancellationToken);
            if (size is null) throw new NotFoundException(nameof(DutySize), request.SizeId);
            entity.Size = size;
        }

        if (request.PriorityId.HasValue)
        {
            var priority = await _context.DutyPriorities.FirstOrDefaultAsync(
                x => x.Id == request.PriorityId, 
                cancellationToken);
            if (priority is null) throw new NotFoundException(nameof(DutyPriority), request.PriorityId);
            entity.Priority = priority;
        }

        var category = await _context.DutyCategories.FirstOrDefaultAsync(
            x => x.Id == request.CategoryId, 
            cancellationToken);
        if (category is null) throw new NotFoundException(nameof(DutyCategory), request.CategoryId);
        entity.Category = category;

        var project = await _context.Projects.FirstOrDefaultAsync(
            x => x.Id == request.ProjectId, 
            cancellationToken);
        if (project is null) throw new NotFoundException(nameof(Project), request.ProjectId);
        entity.Project = project;

        foreach (var userMail in request.Helpers)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(
                x => x.Email == userMail && x.TenantId == _currentUserService.TenantId, 
                cancellationToken);
            if (user is null) continue;
            entity.Helpers.Add(user);
        }

        entity.Order = await GetBiggestOrder(cancellationToken) + 1;

        _context.Duties.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    private async Task<int> GetBiggestOrder(CancellationToken cancellationToken)
    {
        return await _context.Duties
            .OrderByDescending(x => x.Order)
            .Take(1)
            .Select(x => x.Order)
            .FirstOrDefaultAsync(cancellationToken);
    }
}