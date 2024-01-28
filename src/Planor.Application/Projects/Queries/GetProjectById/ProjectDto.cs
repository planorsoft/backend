using Planor.Application.Common.Mappings;
using Planor.Application.Currencies.Queries.GetCurrencies;
using Planor.Application.Customers.Queries;
using Planor.Application.Customers.Queries.GetCustomerById;
using Planor.Application.Customers.Queries.GetCustomersWithPagination;
using Planor.Application.Duties.Queries.GetDutyById;
using Planor.Application.Tags.Queries.GetTagsWithPagination;
using Planor.Domain.Entities;

namespace Planor.Application.Projects.Queries.GetProjectById;

public class ProjectDto  : IMapFrom<Project>
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    
    public bool IsCompleted { get; set; }
    
    public int CustomerId { get; set; }
    
    public CustomerSmallDto Customer { get; set; } = null!;
    
    public int? CurrencyId { get; set; }
    
    public CurrencyDto? Currency { get; set; }
    
    public string? Description { get; set; }
    
    public double Price { get; set; }
}