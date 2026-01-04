using PetFlow.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PetFlow.Filters;

public class AcceptHeaderValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        if (request.Path.StartsWithSegments("/api"))
        {
            var acceptHeader = request.Headers["Accept"].ToString();

            if (!string.IsNullOrEmpty(acceptHeader) &&
                !acceptHeader.Contains("application/json") &&
                !acceptHeader.Contains("*/*"))
            {
                throw new InvalidAcceptHeaderException(acceptHeader);
            }
        }

        await next();
    }
}

