using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Apps.Queries.GetCurrentApp;


public class GetCurrentAppQuery : IRequest<AppDto>
{
}

public class GetCurrentAppQueryHandler : IRequestHandler<GetCurrentAppQuery, AppDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCurrentAppQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppDto> Handle(GetCurrentAppQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Apps
            .ProjectTo<AppDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null) throw new NotFoundException(nameof(App));

        return result;
    }
}