using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.DeleteTag;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public DeleteCustomerCommandHandler(IApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .Where(x => x.Id == request.Id)
            .Include(x => x.Contacts)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(Customer), request.Id.ToString());
        }

        _context.Customers.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}