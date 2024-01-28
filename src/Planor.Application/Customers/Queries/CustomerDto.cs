using Planor.Application.Common.Mappings;
using Planor.Application.Currencies.Queries.GetCurrencies;
using Planor.Application.Users.Queries.GetUser;
using Planor.Domain.Entities;

namespace Planor.Application.Customers.Queries;

public class CustomerDto : IMapFrom<Customer>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public bool IsCompany { get; set; }
     
    public string? Address { get; set; } 

    public string? City { get; set; } 
    
    public string? District { get; set; }
    
    public string? PostCode { get; set; }
    
    public string? Country { get; set; } 
    
    public string? PhoneNumber { get; set; }
    
    public string? Website { get; set; }
    
    public string? GovernmentId { get; set; } 
    
    public string? CurrencyCode { get; set; }

    public bool IsPotantial { get; set; }
    
    public string? ImageUri { get; set; }

    public ICollection<ContactDto> Contacts { get; set; } = new List<ContactDto>();
}