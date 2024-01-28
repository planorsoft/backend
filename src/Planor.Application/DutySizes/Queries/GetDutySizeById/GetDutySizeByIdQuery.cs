using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.DutySizes.Queries.GetDutySizeById;

public class GetDutySizeByIdQuery : IRequest<DutySizeDto>
{
    public int Id { get; set; }
}

public class GetDutySizeByIdQueryHandler: IRequestHandler<GetDutySizeByIdQuery, DutySizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDutySizeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DutySizeDto> Handle(GetDutySizeByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.DutySizes
            .ProjectTo<DutySizeDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (result is null) throw new NotFoundException(nameof(DutySize), request.Id);

        return result;
    }
}