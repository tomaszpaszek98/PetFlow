using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

internal sealed class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<NotFoundExceptionHandler> _logger;

    internal NotFoundExceptionHandler(IProblemDetailsService problemDetailsService,
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
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Entity not found.",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            }
        });
    }
}