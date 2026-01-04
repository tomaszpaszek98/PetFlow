using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PetFlow.Exceptions;

namespace PetFlow.ExceptionHandlers;

internal sealed class InvalidAcceptHeaderExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InvalidAcceptHeaderExceptionHandler> _logger;

    public InvalidAcceptHeaderExceptionHandler(ILogger<InvalidAcceptHeaderExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InvalidAcceptHeaderException invalidAcceptHeaderException)
        {
            return false;
        }

        _logger.LogWarning(exception, "Invalid Accept header: {AcceptHeader}",
            invalidAcceptHeaderException.ReceivedAcceptHeader);

        httpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
        httpContext.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Title = "Not Acceptable.",
            Detail = "API only supports 'Accept: application/json'",
            Type = exception.GetType().Name,
            Status = StatusCodes.Status406NotAcceptable
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}

