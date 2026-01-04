using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PetFlow.Infrastructure.Services;

namespace PetFlow.Infrastructure;

public static class ServiceRegistryExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, DummyCurrentUserService>(); // TODO will be replace by proper one
        services.AddTransient<IDateTime, DateTimeService>();
        
        return services;
    }
}