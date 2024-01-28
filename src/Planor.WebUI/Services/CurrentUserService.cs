using System.Security.Claims;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Exceptions;

namespace Planor.WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Email => _httpContextAccessor.HttpContext?.User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
    public string? Name => _httpContextAccessor.HttpContext?.User.FindFirstValue("name");
    public string TenantId => _httpContextAccessor.HttpContext?.User.FindFirstValue("tenant") ?? "shared";
    
}