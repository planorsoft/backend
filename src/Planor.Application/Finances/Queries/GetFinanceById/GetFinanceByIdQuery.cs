using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Finances.Queries.GetFinanceById;

public class GetFinanceByIdQuery : IRequest<FinanceDto>
{
    public int Id { get; set; }
}

public class GetFinanceByIdQueryHandler : IRequestHandler<GetFinanceByIdQuery, FinanceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFinanceByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FinanceDto> Handle(GetFinanceByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Finances
            .ProjectTo<FinanceDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (result is null) throw new NotFoundException(nameof(Finance), request.Id);

        return result;
    }
}