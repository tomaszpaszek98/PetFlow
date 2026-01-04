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
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Any() == true)
                    .ToDictionary(
                        kvp => kvp.Key.ToLowerInvariant(),
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var problemDetails = new ProblemDetails
                {
                    Title = "Validation error.",
                    Detail = "One or more validation errors occurred",
                    Type = "ValidationException",
                    Status = StatusCodes.Status400BadRequest
                };

                problemDetails.Extensions.Add("errors", errors);

                return new BadRequestObjectResult(problemDetails);
            };
        });

        return services;
    }
}

