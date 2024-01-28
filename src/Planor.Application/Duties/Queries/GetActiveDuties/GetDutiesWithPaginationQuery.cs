using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;

namespace Planor.Application.Duties.Queries.GetActiveDuties;

public class GetActiveDutiesQuery : IRequest<List<DutySmallDto>>
{
    public int? ProjectId { get; set; }
}

public class GetDutiesWithPaginationQueryHandler : IRequestHandler<GetActiveDutiesQuery, List<DutySmallDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDutiesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DutySmallDto>> Handle(GetActiveDutiesQuery request, CancellationToken cancellationToken)
    {
        var result = _context.Duties
            .Where(x => x.Completed == false)
            .OrderBy(x => x.Order)
            .AsQueryable();

        if (request.ProjectId.HasValue)
        {
            result = result.Where(x => x.ProjectId == request.ProjectId);
        }
        
        return await result
            .ProjectTo<DutySmallDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}