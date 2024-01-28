using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Entities;

namespace Planor.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    [Column("tenant_id")]
    public string TenantId { get; set; } = null!;

    [Column("created")]
    public long Created { get; set; }

    [Column("created_by")] 
    public string CreatedBy { get; set; } = null!;
    
    [Column("last_modified")]
    public long? LastModified { get; set; }

    [Column("last_modified_by")] 
    public string LastModifiedBy { get; set; } = null!;
    
}