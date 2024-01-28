using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Customers.Commands.CreateContactToCustomer;
using Planor.Application.Customers.Commands.CreateCustomer;
using Planor.Application.Customers.Commands.DeleteContactToCustomer;
using Planor.Application.Customers.Commands.DeleteCustomer;
using Planor.Application.Customers.Commands.DeleteCustomerImage;
using Planor.Application.Customers.Commands.InviteContactToCustomer;
using Planor.Application.Customers.Commands.UpdateCustomer;
using Planor.Application.Customers.Commands.UploadCustomerImage;
using Planor.Application.Customers.Queries;
using Planor.Application.Customers.Queries.GetCustomerById;
using Planor.Application.Customers.Queries.GetCustomersWithPagination;
using Planor.Application.Users.Queries.GetUser;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/customers")]
public class CustomersController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerRead}")]
    public async Task<ActionResult<PaginatedList<CustomerSmallDto>>> GetCustomersWithPagination(
        [FromQuery] GetCustomersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerRead}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        var query = new GetCustomerByIdQuery()
        {
            Id = id
        };
        
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerCreate}")]
    public async Task<ActionResult<int>> Create(CreateCustomerCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.CustomerUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCustomerCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCustomerCommand { Id = id });

        return NoContent();
    }
    
    [HttpPost("{id}/image")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerCreate}")]
    public async Task<ActionResult> UploadImage(int id, [FromForm] IFormFile file)
    {
        var command = new UploadCustomerImageCommand
        {
            CustomerId = id,
            File = file
        };

        var blobUri = await Mediator.Send(command);

        return Ok(blobUri);
    }
    
    [HttpDelete("{id}/image")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerDelete}")]
    public async Task<ActionResult> DeleteImage(int id)
    {
        var command = new DeleteCustomerImageCommand
        {
            Id = id,
        };

        await Mediator.Send(command);
        
        return NoContent();
    }
    
    [HttpPost("{id}/contacts")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerCreate}")]
    public async Task<ActionResult<ContactDto>> CreateContact(int id, CreateContactToCustomerCommand command)
    {
        if (id != command.CustomerId)
        {
            return BadRequest();
        }
        
        return await Mediator.Send(command);
    }
    
    [HttpDelete("{id}/contacts/{email}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerCreate}")]
    public async Task<ActionResult> DeleteContact(int id, string email)
    {
        var command = new DeleteContactToCustomerCommand
        {
            CustomerId = id,
            Email = email
        };
        
        await Mediator.Send(command);
        
        return NoContent();
    }
    
    [HttpPost("{id}/contacts/invite")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerCreate}")]
    public async Task<ActionResult> InviteContact(int id, InviteContactToCustomerCommand command)
    {
        if (id != command.CustomerId)
        {
            return BadRequest();
        }
        
        await Mediator.Send(command);
        
        return NoContent();
    }
    
}