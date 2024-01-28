using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;
using Planor.Application.Tags.Queries.GetTagsWithPagination;

namespace Planor.Application.Customers.Queries.GetCustomersWithPagination;

public class GetCustomersWithPaginationQuery : IRequest<PaginatedList<CustomerSmallDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IsPotential { get; set; } = false;
}

public class GetCustomersWithPaginationQueryHandler : IRequestHandler<GetCustomersWithPaginationQuery, PaginatedList<CustomerSmallDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CustomerSmallDto>> Handle(GetCustomersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .Where(x => x.IsPotantial == request.IsPotential)
            .ProjectTo<CustomerSmallDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}