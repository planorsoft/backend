using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;
using Planor.Infrastructure.Persistence;

namespace Planor.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;
    
    public TenantService(
        IHttpContextAccessor httpContextAccessor, 
        IApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    
    public string GetCurrentTenantName()
    {
        var defaultTenantName = "shared";
        
        if (_httpContextAccessor.HttpContext is null)
            return defaultTenantName;

        var claim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(e => e.Type == "tenant");

        if (claim is null)
            return defaultTenantName;

        var tenant = claim.Value;

        return tenant;
    }

    public bool AnyTenant(string name)
    {
        return _context.Tenants.Any(x => x.Name == name);
    }

    public async Task CreateTenantAsync(string name)
    {
        if (AnyTenant(name)) throw new BadRequestException($"Bu isim ile bir alan adı bulunuyor: {name}");

        var tenant = new Tenant
        {
            Name = name
        };

        _context.Tenants.Add(tenant);

        await _context.SaveChangesAsync(default);
    }
   
}