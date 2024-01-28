using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Customers.Queries;

public class ContactDto : IMapFrom<User>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? AvatarUri { get; set; }
    public string? PhoneNumber { get; set; }
    public string TenantId { get; set; }
}