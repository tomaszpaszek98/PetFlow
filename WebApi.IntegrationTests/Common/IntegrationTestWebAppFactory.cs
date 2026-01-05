using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFlow.Persistence;
using NSubstitute;
using Application.Common.Interfaces;

namespace WebApi.IntegrationTests.Common;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
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
                options.UseSqlite("Data Source=:memory:");
                // Suppress PendingModelChangesWarning - seed data are static, but runtime changes use dynamic values
                options.ConfigureWarnings(w => 
                    w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
            });

            // Mock IDateTime and ICurrentUserService for database initialization
            var dateTimeMock = Substitute.For<IDateTime>();
            dateTimeMock.Now.Returns(DateTime.UtcNow);
            
            var userServiceMock = Substitute.For<ICurrentUserService>();
            userServiceMock.Email.Returns("test@example.com");
            
            services.AddSingleton(dateTimeMock);
            services.AddSingleton(userServiceMock);
        });
    }
}