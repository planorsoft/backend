using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Tenants.Queries.GetTenantByName;

public class TenantDto : IMapFrom<Tenant>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}