using MediatR;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Interfaces;
using Planor.WebUI.Filters;

namespace Planor.WebUI.Controllers;

[ApiController]
[ApiExceptionFilter]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;
    private ICurrentUserService _currentUserService = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ICurrentUserService CurrentUserService => _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
}