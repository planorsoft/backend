using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Events.Queries;

public class AttendeeDto : IMapFrom<User>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? AvatarUri { get; set; }
    public string? CustomerName { get; set; }
    public string TenantId { get; set; } = null!;
}