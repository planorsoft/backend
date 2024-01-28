using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("duty_categories")]
public class DutyCategory : BaseAuditableEntity
{
    [Column("title")]
    public string Title { get; set; } = null!;
    
    // o-m
    public ICollection<Duty> Duties { get; set; } = new List<Duty>();
}