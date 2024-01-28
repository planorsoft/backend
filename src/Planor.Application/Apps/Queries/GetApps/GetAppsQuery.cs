using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Apps.Queries.GetApps;


public class GetAppsQuery : IRequest<List<AppDto>>
{
}

public class GetAppsQueryHandler : IRequestHandler<GetAppsQuery, List<AppDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAppsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AppDto>> Handle(GetAppsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Apps
            .IgnoreQueryFilters()
            .ProjectTo<AppDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return result;
    }
}