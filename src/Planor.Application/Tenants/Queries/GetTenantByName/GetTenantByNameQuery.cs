using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Tenants.Queries.GetTenantByName;

public class GetTenantByNameQuery : IRequest<TenantDto>
{
    public string Name { get; set; } = null!;
}

public class GetTenantByNameQueryHandler : IRequestHandler<GetTenantByNameQuery, TenantDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTenantByNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TenantDto> Handle(GetTenantByNameQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Tenants
            .Where(x => x.Name == request.Name)
            .ProjectTo<TenantDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null) throw new NotFoundException(nameof(Tenant), request.Name);

        return result;
    }
}