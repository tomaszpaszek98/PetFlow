using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PetFlow.ExceptionHandlers;

internal sealed class JsonExceptionHandler : IExceptionHandler
{
    private readonly ILogger<JsonExceptionHandler> _logger;

    public JsonExceptionHandler(ILogger<JsonExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not JsonException)
        {
            return false;
        }

        _logger.LogWarning(exception, "JSON validation error occurred");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Detail = "The request contains unsupported fields."
        };

        var errors = new Dictionary<string, string[]>
        {
            { "$", new[] { "The request contains unsupported fields." } }
        };

        problemDetails.Extensions["errors"] = errors;
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}

