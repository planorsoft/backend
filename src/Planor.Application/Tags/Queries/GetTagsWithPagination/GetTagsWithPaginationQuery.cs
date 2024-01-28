using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;

namespace Planor.Application.Tags.Queries.GetTagsWithPagination;

public class GetTagsWithPaginationQuery: IRequest<PaginatedList<TagDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetTagsWithPaginationQuery, PaginatedList<TagDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TagDto>> Handle(GetTagsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .OrderBy(x => x.Name)
            .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}