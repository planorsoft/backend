namespace Planor.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? Email { get; }
    string? Name { get; }
    string TenantId { get; }
}