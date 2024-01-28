using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Users.Commands.UploadProfileImage;

public class UploadProfileImageCommand : IRequest<string>
{
    public IFormFile File { get; set; } = null!;
}


public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobService _blobService;
    
    public UploadProfileImageCommandHandler(
        UserManager<User> userManager, 
        ICurrentUserService currentUserService,
        IBlobService blobService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _blobService = blobService;
    }

    public async Task<string> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var email = _currentUserService.Email;
        if (email is null) throw new BadRequestException("User not found");
        
        var user = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == email && x.TenantId == _currentUserService.TenantId, cancellationToken: cancellationToken);
        
        if (user is null) throw new NotFoundException(nameof(User), email);

        await _blobService.DeleteIfExistsAsync(user.Avatar);

        var filename = $"{user.UserName}-profile-image";
        var blob = await _blobService.UploadAsync(request.File, filename);

        user.Avatar = blob;

        await _userManager.UpdateAsync(user);

        return blob.Uri;
    }
}