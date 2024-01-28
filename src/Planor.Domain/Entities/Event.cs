using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("events")]
public class Event : BaseAuditableEntity
{
    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }
    
    [Column("location")]
    public string? Location { get; set; }
    
    [Column("start")]
    public long Start { get; set; }
    
    [Column("end")]
    public long? End { get; set; }

    [Column("color")]
    public string Color { get; set; } = null!;
    
    // m-m
    public ICollection<User> Attendee { get; set; } = new List<User>();
    
    // m-o
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}