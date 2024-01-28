using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Apps.Queries;

public class AppDto : IMapFrom<App>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string TenantId { get; set; } = null!;
    
}