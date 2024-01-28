using System.ComponentModel.DataAnnotations.Schema;
using Planor.Domain.Common;

namespace Planor.Domain.Entities;

[Table("mail_templates")]
public class MailTemplate : BaseAuditableEntity
{
    [Column("title")]
    public string Title { get; set; }
    
    [Column("slug")]
    public string Slug { get; set; }

    [Column("body")]
    public string Body { get; set; }
}