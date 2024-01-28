using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("customers")]
public class Customer : BaseAuditableEntity
{
    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Column("is_company")]
    public bool IsCompany { get; set; }
     
    [Column("address")]
    public string? Address { get; set; }

    [Column("city")]
    public string? City { get; set; }
    
    [Column("district")]
    public string? District { get; set; }
    
    [Column("post_code")]
    public string? PostCode { get; set; }
    
    [Column("country")]
    public string? Country { get; set; }
    
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }
    
    [Column("website")]
    public string? Website { get; set; }
    
    [Column("government_id")]
    public string? GovernmentId { get; set; }
    
    [Column("currency_id")] 
    public int? CurrencyId { get; set; }
    public Currency? Currency { get; set; }

    [Column("is_potantial")]
    public bool IsPotantial { get; set; }
    
    [Column("image_id")]
    public int? ImageId { get; set; }
    public Blob? Image { get; set; }
    
    [Column("image_small_id")]
    public int? ImageSmallId { get; set; }
    public Blob? ImageSmall { get; set; }

    // m-m
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    // o-m
    public ICollection<User> Contacts { get; set; } = new List<User>();
    
    // o-m
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    
    // o-m
    public ICollection<Finance> Finances { get; set; } = new List<Finance>();

}