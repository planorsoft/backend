using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.FinanceCategories.Queries;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.FinanceCategories.Queries.GetFinanceCategoryById;

public class GetFinanceCategoryByIdQuery : IRequest<FinanceCategoryDto>
{
    public int Id { get; set; }
}

public class GetFinanceCategoryByIdQueryHandler : IRequestHandler<GetFinanceCategoryByIdQuery, FinanceCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFinanceCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FinanceCategoryDto> Handle(GetFinanceCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.FinanceCategories
            .ProjectTo<FinanceCategoryDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (result is null) throw new NotFoundException(nameof(FinanceCategory), request.Id);

        return result;
    }
}