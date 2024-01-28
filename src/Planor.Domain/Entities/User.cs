using Microsoft.AspNetCore.Identity;

namespace Planor.Domain.Entities;

public class User : IdentityUser
{
    public int? AvatarId { get; set; }
    public Blob? Avatar { get; set; }
    
    public int? AvatarSmallId { get; set; }
    
    public Blob? AvatarSmall { get; set; }
    public string? Name { get; set; }
    public string? EmailConfirmationCode { get; set; }
    public string? TenantId { get; set; } = null!;
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
}