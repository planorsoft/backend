using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;

namespace Planor.Application.Events.Queries.GetEventsWithPagination;

public class GetEventsWithPaginationQuery: IRequest<PaginatedList<EventSmallDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetEventsWithPaginationQueryHandler : IRequestHandler<GetEventsWithPaginationQuery, PaginatedList<EventSmallDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEventsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EventSmallDto>> Handle(GetEventsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Events
            .ProjectTo<EventSmallDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}