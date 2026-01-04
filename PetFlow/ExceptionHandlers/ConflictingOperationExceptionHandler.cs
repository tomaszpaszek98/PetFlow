using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PetFlow.ExceptionHandlers;

internal sealed class ConflictingOperationExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<ConflictingOperationExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ConflictingOperationException conflictException)
        {
            return false;
        }
        logger.LogError(conflictException, "Exception occurred: {Message}", conflictException.Message);

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Conflicting operation.",
                Detail = exception.Message,
                Type = exception.GetType().Name,
                Status = StatusCodes.Status409Conflict
            }
        });
    }
}