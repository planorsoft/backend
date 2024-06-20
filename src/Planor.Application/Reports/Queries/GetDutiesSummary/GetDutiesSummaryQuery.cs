using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;

namespace Planor.Application.Reports.Queries.GetDutiesSummary;

public class GetDutiesSummaryQuery : IRequest<GetDutiesSummaryDto>
{
    
}

public class GetDutiesSummaryQueryHandler : IRequestHandler<GetDutiesSummaryQuery, GetDutiesSummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDutiesSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetDutiesSummaryDto> Handle(GetDutiesSummaryQuery request, CancellationToken cancellationToken)
    {
        var getDutiesSummaryDto = new GetDutiesSummaryDto();

        getDutiesSummaryDto.TotalCount = await _context
            .Duties
            .CountAsync(cancellationToken);
        
        var groupedDuties = await _context
            .Duties
            .Include(x => x.Category)
            .GroupBy(x => x.Category.Title)
            .Select(x => new
            {
                Category = x.Key,
                Count = x.Count()
            })
            .ToListAsync(cancellationToken);
        
        getDutiesSummaryDto.Detail = string.Join(", ", groupedDuties.Select(x => $"{x.Category}: {x.Count}"));
        
        return getDutiesSummaryDto;
    }
}