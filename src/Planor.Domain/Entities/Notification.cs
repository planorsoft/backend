using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("notification")]
public class Notification : BaseAuditableEntity
{
    [Column("job_id")]
    public string JobId { get; set; } = null!;

    [Column("event_id")]
    public int EventId { get; set; }

    public Event Event { get; set; } = null!;
}