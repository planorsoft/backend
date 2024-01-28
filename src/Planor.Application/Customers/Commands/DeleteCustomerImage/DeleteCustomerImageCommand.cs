using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.DeleteCustomerImage;

public class DeleteCustomerImageCommand : IRequest
{
    public int Id { get; set; }
}


public class DeleteCustomerImageCommandHandler : IRequestHandler<DeleteCustomerImageCommand>
{
    private IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobService _blobService;
    
    public DeleteCustomerImageCommandHandler(
        IApplicationDbContext context, 
        ICurrentUserService currentUserService,
        IBlobService blobService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _blobService = blobService;
    }

    public async Task Handle(DeleteCustomerImageCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (customer is null) throw new NotFoundException(nameof(Customer), request.Id);
        
        if (customer.ImageId is null) throw new NotFoundException(nameof(Blob));
        
        await _blobService.DeleteIfExistsAsync(customer.ImageId);

        customer.Image = null;
        await _context.SaveChangesAsync(cancellationToken);
    }
}