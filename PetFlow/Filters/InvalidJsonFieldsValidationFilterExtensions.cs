using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Filters;

internal static class InvalidJsonFieldsValidationFilterExtensions
{
    public static IServiceCollection AddInvalidJsonFieldsValidation(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors
                            .Select(e => 
                            {
                                // Replace detailed .NET type messages with generic message
                                if (e.ErrorMessage.Contains("could not be mapped to any .NET member"))
                                {
                                    return "Unsupported field.";
                                }
                                return e.ErrorMessage;
                            })
                            .ToArray()
                    );

                var problemDetails = new ProblemDetails
                {
                    Title = "One or more validation errors occurred.",
                    Detail = "The request contains validation errors.",
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Status = StatusCodes.Status400BadRequest
                };

                problemDetails.Extensions["errors"] = errors;
                problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                return new BadRequestObjectResult(problemDetails);
            };
        });

        return services;
    }
}

