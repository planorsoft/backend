using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.DutyCategories.Queries.GetDutyCategories;

public class DutyCategorySmallDto : IMapFrom<DutyCategory>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
}