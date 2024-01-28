using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("duty_priorities")]
public class DutyPriority : BaseAuditableEntity
{
    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Column("score")]
    public int Score { get; set; }
    
    // o-m
    public ICollection<Duty> Duties { get; set; } = new List<Duty>();
}