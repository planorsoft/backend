using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("app")]
public class App : BaseAuditableEntity
{
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("code")] 
    public string Code { get; set; } = null!;

}