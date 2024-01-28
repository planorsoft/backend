using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.DutyCategories.Queries.GetDutyCategoryById;

public class DutyCategoryDto : IMapFrom<DutyCategory>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
}