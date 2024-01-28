using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("duty_todos")]
public class DutyTodo : BaseAuditableEntity
{
    [Column("content")]
    public string Content { get; set; } = null!;
    
    [Column("is_completed")]
    public bool IsCompleted { get; set; }

    [Column("duty_id")]
    public int DutyId { get; set; }
    public Duty Duty { get; set; } = null!;
}