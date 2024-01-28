using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;


namespace Planor.Application.DutyCategories.Queries.GetDutyCategories;

public class GetDutyCategoriesQuery : IRequest<List<DutyCategorySmallDto>>
{
}

public class GetDutyCategoriesQueryHandler: IRequestHandler<GetDutyCategoriesQuery, List<DutyCategorySmallDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDutyCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DutyCategorySmallDto>> Handle(GetDutyCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.DutyCategories
            .ProjectTo<DutyCategorySmallDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    }
}