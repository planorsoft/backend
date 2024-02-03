using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Planor.Application.Common.Interfaces;

namespace Planor.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        var requestName = typeof(TRequest).Name;

        try
        {
            var response = await next();

            _logger.LogInformation("Planor Request: {Name} {@Request}", requestName, request);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("Planor Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
            throw;
            return await next();
        }
    }
}