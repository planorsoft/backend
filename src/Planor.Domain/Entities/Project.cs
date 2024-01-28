using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("projects")]
public class Project : BaseAuditableEntity
{
    [Column("title")]
    public string Title { get; set; } = null!;
    
    [Column("is_completed")]
    public bool IsCompleted { get; set; }
    
    [Column("customer_id")] 
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("price")]
    public double Price { get; set; }
    
    [Column("currency_id")]
    public int? CurrencyId { get; set; }
    public Currency? Currency { get; set; }
    
    // m-m
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    // o-m
    public ICollection<Duty> Duties { get; set; } = new List<Duty>();
    
    // o-m
    public ICollection<Finance> Finances { get; set; } = new List<Finance>();

}