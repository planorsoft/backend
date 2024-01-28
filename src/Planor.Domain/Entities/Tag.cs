using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("tags")]
public class Tag : BaseAuditableEntity
{
    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Column("slug")]
    public string Slug { get; set; } = null!;
}