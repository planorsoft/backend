using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Users.Queries.GetUser.GetTeam;

public class GetTeamQuery: IRequest<List<UserSmallDto>>
{
}

public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, List<UserSmallDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetTeamQueryHandler(IApplicationDbContext context, UserManager<User> userManager, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<UserSmallDto>> Handle(GetTeamQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .Where(x => x.TenantId == _currentUserService.TenantId && x.CustomerId == null)
            .ToListAsync(cancellationToken);

        var roles = new Dictionary<string, IList<string>>();
        
        foreach (var user in users)
        {
            if (user.Email is null) continue;
            var userRoles = await _userManager.GetRolesAsync(user);
            roles.Add(user.Email, userRoles);
        }

        var userDtoList = _mapper.Map<List<UserSmallDto>>(users); 

        foreach (var userDto in userDtoList)
        {
            if (userDto.Email is null) continue;
            userDto.RoleName = roles[userDto.Email].FirstOrDefault(x => Domain.Constants.Roles.List.Contains(x));
        }
        
        return userDtoList;
    }
}