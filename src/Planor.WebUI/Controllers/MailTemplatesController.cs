using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.MailTemplates.Commands.CreateMailTemplate;
using Planor.Application.MailTemplates.Commands.DeleteMailTemplate;
using Planor.Application.MailTemplates.Commands.UpdateMailTemplate;
using Planor.Application.MailTemplates.Queries.GetMailTemplates;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/mail-templates")]
public class MailTemplatesController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.MailTemplateRead}")]
    public async Task<ActionResult<List<MailTemplateDto>>> GetMailTemplates([FromQuery] GetMailTemplatesQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.MailTemplateCreate}")]
    public async Task<ActionResult<int>> Create(CreateMailTemplateCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.MailTemplateUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateMailTemplateCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.MailTemplateDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMailTemplateCommand { Id = id });

        return NoContent();
    }
}