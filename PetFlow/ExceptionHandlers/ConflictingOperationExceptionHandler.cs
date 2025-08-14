using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PetFlow.ExceptionHandlers;

internal sealed class ConflictingOperationExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<ConflictingOperationExceptionHandler> _logger;

    internal ConflictingOperationExceptionHandler(IProblemDetailsService problemDetailsService,
        ILogger<ConflictingOperationExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ConflictingOperationException conflictException)
        {
            return false;
        }
        _logger.LogError(conflictException, "Exception occurred: {Message}", conflictException.Message);

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Conflicting operation.",
                Detail = exception.Message,
                Status = StatusCodes.Status409Conflict
            }
        });
    }
}