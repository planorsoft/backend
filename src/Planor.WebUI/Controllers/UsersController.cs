using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Users.Commands.CreateUser;
using Planor.Application.Users.Commands.DeleteProfileImage;
using Planor.Application.Users.Commands.UpdateUser;
using Planor.Application.Users.Commands.UploadProfileImage;
using Planor.Application.Users.Queries.GetUser;
using Planor.Application.Users.Queries.GetUser.GetTeam;
using Planor.Domain.Constants;
using Planor.Domain.Exceptions;

namespace Planor.WebUI.Controllers;


[Route("api/users")]
public class UsersController : ApiControllerBase
{
    [HttpPost("image")]
    [Authorize]
    public async Task<ActionResult> UploadProfileImage([FromForm] IFormFile file)
    {
        var command = new UploadProfileImageCommand
        {
            File = file
        };

        var blobUri = await Mediator.Send(command);

        return Ok(blobUri);
    }
    
    [HttpDelete("image")]
    [Authorize]
    public async Task<ActionResult> DeleteProfileImage()
    {
        var command = new DeleteProfileImageCommand();

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpGet("team")]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Admin},{Roles.Employee}")]
    public async Task<ActionResult<List<UserSmallDto>>> GetTeam()
    {
        return await Mediator.Send(new GetTeamQuery());
    }

    [HttpGet("{email}")]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Admin},{Roles.Employee}")]
    public async Task<ActionResult<UserDto>> GetUserWithId(string email)
    {
        var query = new GetUserQuery
        {
            Email = email
        };

        return await Mediator.Send(query);
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetUserWithId()
    {
        if (CurrentUserService.Email is null)
        {
            return BadRequest();
        }
        
        var query = new GetUserQuery
        {
            Email = CurrentUserService.Email
        };

        return await Mediator.Send(query);
    }
    
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.Employee}")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{email}")]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Admin},{Roles.Employee}")]
    public async Task<ActionResult> UpdateUser(string email, [FromBody] UpdateUserCommand command)
    {
        if (email != command.Email)
        {
            return BadRequest();
        }
        
        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpPost("me")]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        var email = CurrentUserService.Email;
        if (email is null) throw new BadRequestException("Email is null");

        command.Email = email;
        
        await Mediator.Send(command);

        return NoContent();
    }
}