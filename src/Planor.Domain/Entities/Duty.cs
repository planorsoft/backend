using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("duties")]
public class Duty : BaseAuditableEntity
{
    [Column("title")]
    public string Title { get; set; } = null!;
    
    [Column("description")]
    public string Description { get; set; } = null!;
    
    [Column("size_id")]
    public int? SizeId { get; set; }
    public DutySize? Size { get; set; }
    
    [Column("priority_id")] 
    public int? PriorityId { get; set; }
    public DutyPriority? Priority { get; set; }

    [Column("category_id")] 
    public int CategoryId { get; set; }
    public DutyCategory Category { get; set; } = null!;
    
    [Column("project_id")] 
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    [Column("completed")]
    public bool Completed { get; set; }
    
    [Column("order")]
    public int Order { get; set; }
    
    [Column("assigned_to")] 
    public string? AssignedTo { get; set; }

    // o-m
    public ICollection<DutyTodo> Todos { get; set; } = new List<DutyTodo>();
    
    // m-m
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}