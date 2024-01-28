using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Users.Commands.UploadProfileImage;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Users.Commands.DeleteProfileImage;

public class DeleteProfileImageCommand : IRequest
{
}


public class DeleteProfileImageCommandHandler : IRequestHandler<DeleteProfileImageCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobService _blobService;
    
    public DeleteProfileImageCommandHandler(
        UserManager<User> userManager, 
        ICurrentUserService currentUserService,
        IBlobService blobService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _blobService = blobService;
    }

    public async Task Handle(DeleteProfileImageCommand request, CancellationToken cancellationToken)
    {
        var email = _currentUserService.Email;
        if (email is null) throw new BadRequestException("User not found");
        
        var user = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == email && x.TenantId == _currentUserService.TenantId, cancellationToken: cancellationToken);
        
        if (user is null) throw new NotFoundException(nameof(User), email);

        if (user.AvatarId is null) throw new NotFoundException(nameof(Blob));
        
        await _blobService.DeleteIfExistsAsync(user.AvatarId);

        user.Avatar = null;

        await _userManager.UpdateAsync(user);
    }
}