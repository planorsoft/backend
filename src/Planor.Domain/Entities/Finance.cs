using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("finances")]
public class Finance : BaseAuditableEntity
{
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("amount")]
    public float Amount { get; set; }
    
    [Column("is_income")] 
    public bool IsIncome { get; set; }

    [Column("finance_category_id")]
    public int CategoryId { get; set; }
    
    [Column("date")]
    public long Date { get; set; }

    public FinanceCategory Category { get; set; } = new FinanceCategory();
    
    [Column("customer_id")]
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [Column("project_id")]
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
}