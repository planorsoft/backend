using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.MailTemplates.Queries.GetMailTemplates;

namespace Planor.Application.Currencies.Queries.GetCurrencies;

public class GetCurrenciesQuery: IRequest<List<CurrencyDto>>
{
    
}

public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, List<CurrencyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCurrenciesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Currencies
            .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}