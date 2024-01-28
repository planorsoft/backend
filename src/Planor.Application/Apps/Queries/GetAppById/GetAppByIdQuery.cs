using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Apps.Queries.GetAppById;

public class GetAppByIdQuery : IRequest<AppDto>
{
    public int Id { get; set; }
}

public class GetAppByIdQueryHandler : IRequestHandler<GetAppByIdQuery, AppDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAppByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppDto> Handle(GetAppByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Apps
            .IgnoreQueryFilters()
            .ProjectTo<AppDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (result is null) throw new NotFoundException(nameof(App), request.Id);

        return result;
    }
}