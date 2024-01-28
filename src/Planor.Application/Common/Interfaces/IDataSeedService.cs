namespace Planor.Application.Common.Interfaces;

public interface IDataSeedService
{
    public Task SeedAsync(string tenantId);

}