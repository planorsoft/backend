using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.FinanceCategories.Queries;

public class FinanceCategoryDto : IMapFrom<FinanceCategory>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}