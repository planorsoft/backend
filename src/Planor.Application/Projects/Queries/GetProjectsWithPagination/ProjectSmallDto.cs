using Planor.Application.Common.Mappings;
using Planor.Application.Customers.Queries.GetCustomerById;
using Planor.Application.Customers.Queries.GetCustomersWithPagination;
using Planor.Domain.Entities;

namespace Planor.Application.Projects.Queries.GetProjectsWithPagination;

public class ProjectSmallDto  : IMapFrom<Project>
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public bool IsCompleted { get; set; }

    public double Price { get; set; }
    
    public string? CurrencySymbol { get; set; }
}