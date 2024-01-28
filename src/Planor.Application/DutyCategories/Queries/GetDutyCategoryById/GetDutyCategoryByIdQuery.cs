using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.DutyCategories.Queries.GetDutyCategoryById;

public class GetDutyCategoryByIdQuery : IRequest<DutyCategoryDto>
{
    public int Id { get; set; }
}

public class GetDutyCategoryByIdQueryHandler: IRequestHandler<GetDutyCategoryByIdQuery, DutyCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDutyCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DutyCategoryDto> Handle(GetDutyCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.DutyCategories
            .ProjectTo<DutyCategoryDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (result is null) throw new NotFoundException(nameof(DutyCategory), request.Id);

        return result;
    }
}