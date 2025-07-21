using Microsoft.Extensions.DependencyInjection;

namespace PetFlow.Infrastructure;

public static class ServiceRegistryExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}