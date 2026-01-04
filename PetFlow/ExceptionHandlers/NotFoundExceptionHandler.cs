using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

internal sealed class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<NotFoundExceptionHandler> _logger;

    public NotFoundExceptionHandler(IProblemDetailsService problemDetailsService,
        ILogger<NotFoundExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }
        
        _logger.LogError(notFoundException, "Exception occurred: {Message}", notFoundException.Message);
        
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Title = "Entity not found.",
                Detail = exception.Message,
                Type = exception.GetType().Name,
                Status = StatusCodes.Status404NotFound
            }
        });
    }
}