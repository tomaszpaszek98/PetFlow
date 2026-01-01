using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetFlow.Persistence;

public static class ServiceRegistryExtensions
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PetFlowFlowDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("PetDatabase")));
        services.AddScoped<IPetFlowDbContext, PetFlowFlowDbContext>();

        return services;
    }
}