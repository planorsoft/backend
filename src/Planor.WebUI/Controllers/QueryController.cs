

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Planor.Application.Common.Interfaces;
using Planor.Application.Currencies.Queries.GetCurrencies;
using Planor.Application.Customers.Queries;
using Planor.Application.Customers.Queries.GetCustomerById;
using Planor.Application.Duties.Queries.GetDutyById;
using Planor.Application.Events.Queries;
using Planor.Application.Projects.Queries.GetProjectById;
using Planor.Application.Tags.Queries.GetTagsWithPagination;
using Planor.Application.Users.Queries.GetUser;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.WebUI.Controllers;

[Route("api/odata")]
public class QueryController : ODataController
{

    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public QueryController(
        IApplicationDbContext context,
        UserManager<User> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }
    
    [HttpGet("users")]
    [Authorize]
    [EnableQuery]
    [ODataAttributeRouting]
    public IQueryable<UserDto> GetUsers([FromQuery] string tenant)
    {
        if (tenant is null) throw new BadRequestException("tenant must not be null");
        
        var result = _userManager
            .Users
            .Where(x => x.TenantId == tenant)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider);

        return result;
    }
    
    [HttpGet("customers")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerRead}")]
    [EnableQuery]
    [ODataAttributeRouting]
    public IQueryable<CustomerDto> GetCustomers()
    {
        var result = _context.Customers
            .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider);

        return result;
    }
    
    [HttpGet("currencies")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CurrencyRead}")]
    [EnableQuery]
    [ODataAttributeRouting]
    public IQueryable<CurrencyDto> GetCurrencies()
    {
        var result = _context.Currencies
            .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider);

        return result;
    }
    
    [HttpGet("duties")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    [EnableQuery]
    [ODataAttributeRouting]
    public IQueryable<DutyDto> GetDuties()
    {
        var result = _context.Duties
            .ProjectTo<DutyDto>(_mapper.ConfigurationProvider);

        return result;
    }
    
    [HttpGet("projects")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectRead}")]
    [EnableQuery]
    [ODataAttributeRouting]
    public IQueryable<ProjectDto> GetProjects()
    {
        var result = _context.Projects
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider);

        return result;
    }
    
    [HttpGet("tags")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.TagRead}")]
    [EnableQuery]
    [ODataAttributeRouting]
    public IQueryable<TagDto> GetTags()
    {
        var result = _context.Tags
            .ProjectTo<TagDto>(_mapper.ConfigurationProvider);

        return result;
    }
    
    [HttpGet("events")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.EventRead}")]
    [EnableQuery]
    [ODataAttributeRouting]
    public IQueryable<EventDto> GetEvents()
    {
        var result = _context.Events
            .ProjectTo<EventDto>(_mapper.ConfigurationProvider);

        return result;
    }
}