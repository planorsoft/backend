using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Finances.Queries;

public class FinanceDto : IMapFrom<Finance>
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public float Amount { get; set; }
    public long Date { get; set; }
    public int? CustomerId { get; set; }
    public int? ProjectId { get; set; }
}