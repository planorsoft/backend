using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Duties.Queries.GetDutyById;

public class GetDutyByIdQuery : IRequest<DutyDto>
{
    public int Id { get; set; }
}

public class GetDutyByIdQueryHandler : IRequestHandler<GetDutyByIdQuery, DutyDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDutyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DutyDto> Handle(GetDutyByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Duties
            .ProjectTo<DutyDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (result is null) throw new NotFoundException(nameof(Duty), request.Id);

        return result;
    }
}