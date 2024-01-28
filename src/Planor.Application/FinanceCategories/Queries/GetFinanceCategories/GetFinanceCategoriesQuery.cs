using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;

namespace Planor.Application.FinanceCategories.Queries.GetFinanceCategories;

public class GetFinanceCategoriesQuery : IRequest<List<FinanceCategoryDto>>
{
}

public class GetFinanceCategoriesQueryHandler : IRequestHandler<GetFinanceCategoriesQuery, List<FinanceCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFinanceCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FinanceCategoryDto>> Handle(GetFinanceCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.FinanceCategories
            .ProjectTo<FinanceCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}