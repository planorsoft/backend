using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("blob")]
public class Blob : BaseAuditableEntity
{
    [Column("name")]
    public string Name { get; set; }
    
    [Column("uri")]
    public string Uri { get; set; }

    public Blob(string name, string uri)
    {
        Name = name;
        Uri = uri;
    }
}