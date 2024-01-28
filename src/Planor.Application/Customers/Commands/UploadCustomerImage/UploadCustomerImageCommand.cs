using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Users.Commands.UploadProfileImage;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.UploadCustomerImage;

public class UploadCustomerImageCommand: IRequest<string>
{
    public int CustomerId { get; set; }
    public IFormFile File { get; set; } = null!;
}


public class UploadCustomerImageCommandHandler : IRequestHandler<UploadCustomerImageCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobService _blobService;
    
    public UploadCustomerImageCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IBlobService blobService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _blobService = blobService;
    }

    public async Task<string> Handle(UploadCustomerImageCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null) throw new NotFoundException(nameof(Customer), request.CustomerId);
        
        await _blobService.DeleteIfExistsAsync(customer.Image);

        var filename = $"{customer.Name}-{Guid.NewGuid()}";
        var blob = await _blobService.UploadAsync(request.File, filename);

        customer.Image = blob;
        await _context.SaveChangesAsync(cancellationToken);
        
        return blob.Uri;
    }
}