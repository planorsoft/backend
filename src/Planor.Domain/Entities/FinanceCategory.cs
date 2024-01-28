using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("finance_categories")]
public class FinanceCategory : BaseAuditableEntity
{
    [Column("name")]
    public string Name { get; set; } = null!;
    public ICollection<Finance> Finances { get; set; } = new List<Finance>();
}