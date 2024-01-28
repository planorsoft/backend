using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Identity.Commands.ConfirmEmail;
using Planor.Application.Identity.Commands.Forgot;
using Planor.Application.Identity.Commands.ForgotConfirm;
using Planor.Application.Identity.Commands.Invite;
using Planor.Application.Identity.Commands.Login;
using Planor.Application.Identity.Commands.Register;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/identity")]
public class IdentityController : ApiControllerBase
{
    
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterCommand command)
    {
        var succeed = await Mediator.Send(command);

        if (!succeed) return BadRequest("Hesap oluşturulamadı.");

        return Ok("Lütfen mail adresinizi kontrol edin.");
    }

    [HttpPost("forgot")]
    public async Task<ActionResult<bool>> Forgot(ForgotCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPost("forgot-confirm")]
    public async Task<ActionResult<bool>> Forgot(ForgotConfirmCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("invite")]
    [Authorize(Roles = Roles.Manager)]
    public async Task<ActionResult<bool>> Invite(InviteCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginDto>> Login(LoginCommand command)
    {
        return await Mediator.Send(command);
    }
    

    [HttpPost("confirm")]
    public async Task<ActionResult<bool>> ConfirmEmail(ConfirmEmailCommand command)
    {
        return await Mediator.Send(command);
    }

}