using Planor.Domain.Entities;

namespace Planor.Application.Common.Interfaces;

public interface ITenantService
{
    bool AnyTenant(string name);

    Task CreateTenantAsync(string name);
}