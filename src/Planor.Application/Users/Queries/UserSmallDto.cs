using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Users.Queries.GetUser;

public class UserSmallDto : IMapFrom<User>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? AvatarUri { get; set; }
    public string? PhoneNumber { get; set; }
    public string TenantId { get; set; }
    public string? RoleName { get; set; }
}