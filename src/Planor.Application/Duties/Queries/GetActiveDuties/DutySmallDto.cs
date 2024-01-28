using Planor.Application.Common.Mappings;
using Planor.Application.Tags.Queries.GetTagsWithPagination;
using Planor.Domain.Entities;

namespace Planor.Application.Duties.Queries.GetActiveDuties;

public class DutySmallDto : IMapFrom<Duty>
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string Code { get; set; } = null!;
    
    public int SizeId { get; set; }
    
    public int PriorityId { get; set; }

    public int CategoryId { get; set; }
    
    public int ProjectId { get; set; }
    
    public bool Completed { get; set; }
    
    public int Order { get; set; }

    public string AssignedTo { get; set; } = null!;
}