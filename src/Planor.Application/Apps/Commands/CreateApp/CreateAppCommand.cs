using System.Text.RegularExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Apps.Commands.CreateApp;

public class CreateAppCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
}

public class CreateAppCommandHandler : IRequestHandler<CreateAppCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateAppCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateAppCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var isFoundWithTenant = await _context.Apps.AnyAsync(x => x.TenantId == tenantId, cancellationToken);
        if (isFoundWithTenant) throw new BadRequestException("Bu tenant ile bir uygulama zaten oluşturulmuş.");

        var entity = new App
        {
            Name = request.Name,
        };

        _context.Apps.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}