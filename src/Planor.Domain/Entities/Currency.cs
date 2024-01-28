using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("currencies")]
public class Currency : BaseAuditableEntity
{
    [Column("code")]
    public string Code { get; set; } = null!;
    
    [Column("symbol")]
    public string? Symbol { get; set; }
    
    [Column("rate")]
    public double Rate { get; set; }

    [Column("isDefault")] 
    public bool IsDefault { get; set; }
    
    // o-m
    public ICollection<Project> Projects { get; set; } = new List<Project>();

}