using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Tags.Queries.GetTagsWithPagination;

public class TagDto : IMapFrom<Tag>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
}