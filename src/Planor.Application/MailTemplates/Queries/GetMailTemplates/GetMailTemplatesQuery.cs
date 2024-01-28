using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Common.Mappings;
using Planor.Application.Common.Models;
using Planor.Application.Tags.Queries.GetTagsWithPagination;

namespace Planor.Application.MailTemplates.Queries.GetMailTemplates;

public class GetMailTemplatesQuery: IRequest<List<MailTemplateDto>>
{
    
}

public class GetMailTemplatesQueryHandler : IRequestHandler<GetMailTemplatesQuery, List<MailTemplateDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMailTemplatesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MailTemplateDto>> Handle(GetMailTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.MailTemplates
            .OrderBy(x => x.Id)
            .ProjectTo<MailTemplateDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}