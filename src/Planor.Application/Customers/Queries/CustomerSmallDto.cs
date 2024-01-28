using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Customers.Queries;

public class CustomerSmallDto : IMapFrom<Customer>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? City { get; set; } 
    
    public bool IsCompany { get; set; }
    
    public bool IsPotantial { get; set; }
    
    public string? ImageUri { get; set; }
}