using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Finances.Queries;

public class FinanceSmallDto : IMapFrom<Finance>
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public float Amount { get; set; }
}