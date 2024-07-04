using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;

namespace Planor.Application.Reports.Queries.GetCustomersSummary;

public class GetCustomersSummaryQuery : IRequest<GetCustomersSummaryDto>
{
    
}

public class GetCustomersSummaryQueryHandler : IRequestHandler<GetCustomersSummaryQuery, GetCustomersSummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomersSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCustomersSummaryDto> Handle(GetCustomersSummaryQuery request, CancellationToken cancellationToken)
    {
        var getCustomersSummaryDto = new GetCustomersSummaryDto();

        getCustomersSummaryDto.TotalCount = await _context
            .Customers
            .CountAsync(cancellationToken);
        
        getCustomersSummaryDto.PotentialCount = await _context.
            Customers
            .Where(x => x.IsPotantial == true)
            .CountAsync(cancellationToken);
        
        return getCustomersSummaryDto;
    }
}