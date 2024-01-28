using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.DutySizes.Queries;

public class DutySizeDto : IMapFrom<DutySize>
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public int Score { get; set; }
}