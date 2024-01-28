using Planor.Application.Common.Mappings;
using Planor.Application.Customers.Queries;
using Planor.Domain.Entities;

namespace Planor.Application.Users.Queries.GetUser;

public class UserDto : IMapFrom<User>
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? AvatarUri { get; set; }
    public string? PhoneNumber { get; set; }
    public CustomerSmallDto? Customer { get; set; }
    public string TenantId { get; set; }
}