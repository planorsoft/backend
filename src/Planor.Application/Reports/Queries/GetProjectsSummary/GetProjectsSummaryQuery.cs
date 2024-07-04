using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;
using Planor.Application.Tags.Queries.GetTagsWithPagination;

namespace Planor.Application.Reports.Queries.GetProjectsSummary;

public class GetProjectsSummaryQuery : IRequest<GetProjectsSummaryDto>
{
    
}

public class GetProjectsSummaryQueryHandler : IRequestHandler<GetProjectsSummaryQuery, GetProjectsSummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectsSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetProjectsSummaryDto> Handle(GetProjectsSummaryQuery request, CancellationToken cancellationToken)
    {
        var getProjectsSummaryDto = new GetProjectsSummaryDto();

        getProjectsSummaryDto.CompletedCount = await _context
            .Projects
            .Where(x => x.IsCompleted == true)
            .CountAsync(cancellationToken);
        
        getProjectsSummaryDto.ActiveCount = await _context.
            Projects
            .Where(x => x.IsCompleted == false)
            .CountAsync(cancellationToken);
        
        getProjectsSummaryDto.TotalCount = getProjectsSummaryDto.CompletedCount + getProjectsSummaryDto.ActiveCount; 

        return getProjectsSummaryDto;
    }
}