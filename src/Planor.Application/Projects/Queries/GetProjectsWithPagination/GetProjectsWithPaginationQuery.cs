using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;

namespace Planor.Application.Projects.Queries.GetProjectsWithPagination;

public class GetProjectsWithPaginationQuery : IRequest<PaginatedList<ProjectSmallDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetProjectsWithPaginationQueryHandler : IRequestHandler<GetProjectsWithPaginationQuery, PaginatedList<ProjectSmallDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProjectSmallDto>> Handle(GetProjectsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .ProjectTo<ProjectSmallDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}