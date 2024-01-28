using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Customers.Commands.DeleteContactToCustomer;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.DeleteContactToCustomer;

public class DeleteContactToCustomerCommand: IRequest
{
    public int CustomerId { get; set; }
    public string Email { get; set; } = null!;
}

public class DeleteContactToCustomerCommandHandler : IRequestHandler<DeleteContactToCustomerCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public DeleteContactToCustomerCommandHandler(
        IApplicationDbContext context, 
        UserManager<User> userManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteContactToCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null) throw new NotFoundException(nameof(Customer), request.CustomerId);
        
        var user = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == request.Email && x.TenantId == _currentUserService.TenantId, cancellationToken: cancellationToken);
        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        customer.Contacts.Remove(user);
        await _userManager.DeleteAsync(user);
    }
}