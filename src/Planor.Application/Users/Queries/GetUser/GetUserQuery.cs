using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Users.Queries.GetUser;

public class GetUserQuery: IRequest<UserDto>
{
    public string Email { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IApplicationDbContext context, UserManager<User> userManager, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x =>
                x.Email == request.Email 
                && x.TenantId == _currentUserService.TenantId, 
                cancellationToken: cancellationToken);
        
        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        return user;
    }
}