using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("tenant")]
public class Tenant : BaseEntity
{
    [Column("name")]
    public string Name { get; set; } = null!;
}