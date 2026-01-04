using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Filters;

internal static class InvalidJsonFieldsValidationFilterExtensions
{
    public static IServiceCollection AddInvalidJsonFieldsValidation(
        this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Validation error.",
                    Detail = "Unexpected field in request body.",
                    Type = "ValidationException",
                    Status = StatusCodes.Status400BadRequest
                };


                return new BadRequestObjectResult(problemDetails);
            };
        });

        return services;
    }
}

