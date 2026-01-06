using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFlow.Persistence;
using Testcontainers.PostgreSql;

namespace WebApi.IntegrationTests.Common;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    private PostgreSqlContainer? _dbContainer;
    private string? _connectionString;
    
    public async Task InitializeAsync()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("petflow_test_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _dbContainer.StartAsync();
        _connectionString = _dbContainer.GetConnectionString();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<PetFlowDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<PetFlowDbContext>(options =>
            {
                options.UseNpgsql(_connectionString);
            });
        });
    }

    public new async Task DisposeAsync()
    {
        if (_dbContainer is not null)
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }
    }
}