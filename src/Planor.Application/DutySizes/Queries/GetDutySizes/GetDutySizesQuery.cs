using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;

namespace Planor.Application.DutySizes.Queries.GetDutySizes;

public class GetDutySizesQuery : IRequest<List<DutySizeDto>>
{
    
}

public class GetDutySizesQueryHandler: IRequestHandler<GetDutySizesQuery, List<DutySizeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDutySizesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DutySizeDto>> Handle(GetDutySizesQuery request, CancellationToken cancellationToken)
    {
        return await _context.DutySizes
            .ProjectTo<DutySizeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    }
}