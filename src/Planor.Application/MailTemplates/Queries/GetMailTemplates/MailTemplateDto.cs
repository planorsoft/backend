using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.MailTemplates.Queries.GetMailTemplates;

public class MailTemplateDto : IMapFrom<MailTemplate>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
}