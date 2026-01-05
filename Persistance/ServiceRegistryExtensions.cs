using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFlow.Persistence.Repositories;

namespace PetFlow.Persistence;

public static class ServiceRegistryExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PetFlowDbContext>(options => options.UseSqlite(configuration.GetConnectionString("PetDatabase")));
        
        services.AddScoped<IPetFlowDbContext, PetFlowDbContext>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IMedicalNoteRepository, MedicalNoteRepository>();
        
        return services;
    }
}