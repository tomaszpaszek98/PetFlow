using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using PetFlow.Persistance.Repositories;

namespace PetFlow.Persistance;

public static class ServiceRegistryExtensions
{
    public static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        services.AddSingleton<IPetRepository, PetRepository>();

        return services;
    }
}