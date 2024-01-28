using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;

namespace Planor.Application.Finances.Queries.GetFinanceWithPagination;

public class GetFinancesWithPaginationQuery : IRequest<PaginatedList<FinanceSmallDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IsIncome { get; set; } = false;
}

public class GetFinancesWithPaginationQueryHandler : IRequestHandler<GetFinancesWithPaginationQuery, PaginatedList<FinanceSmallDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFinancesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FinanceSmallDto>> Handle(GetFinancesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Finances
            .Where(x => x.IsIncome == request.IsIncome)
            .ProjectTo<FinanceSmallDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}