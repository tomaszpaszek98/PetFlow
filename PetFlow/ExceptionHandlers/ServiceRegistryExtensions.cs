namespace PetFlow.ExceptionHandlers;

internal static class ServiceRegistryExtensions
{
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<ConflictingOperationExceptionHandler>();
        
        // GlobalExceptionHandler must be at the end of the chain!
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
        return services;
    }
}