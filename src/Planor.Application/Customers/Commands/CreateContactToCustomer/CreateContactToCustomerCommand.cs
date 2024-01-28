using System.Globalization;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Customers.Queries;
using Planor.Application.Users.Queries.GetUser;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.CreateContactToCustomer;

public class CreateContactToCustomerCommand : IRequest<ContactDto>
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    
    public string? PhoneNumber { get; set; }
}

public class CreateContactToCustomerCommandHandler : IRequestHandler<CreateContactToCustomerCommand, ContactDto>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly CultureInfo _culture;

    public CreateContactToCustomerCommandHandler(
        IApplicationDbContext context, 
        UserManager<User> userManager,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _culture = new CultureInfo("en-US");
    }

    public async Task<ContactDto> Handle(CreateContactToCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null) throw new NotFoundException(nameof(Customer), request.CustomerId);
        
        var userIsFoundWithMail = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == request.Email && x.TenantId == _currentUserService.TenantId, cancellationToken: cancellationToken);
        if (userIsFoundWithMail is not null) throw new BadRequestException("Bu mail adresiyle bir kullanıcı bulunuyor");
        
        var username = $"{request.Email}-{_currentUserService.TenantId}";

        var user = new User
        {
            Name = _culture.TextInfo.ToTitleCase(request.Name),
            UserName = username,
            Email = request.Email,
            TenantId = _currentUserService.TenantId,
            EmailConfirmed = true,
            Customer = customer,
            PhoneNumber = request.PhoneNumber
        };

        await _userManager.CreateAsync(user);
        
        await _userManager.AddToRolesAsync(user, Domain.Constants.Roles.CustomerRoles);
        
        return _mapper.Map<ContactDto>(user);
    }
}