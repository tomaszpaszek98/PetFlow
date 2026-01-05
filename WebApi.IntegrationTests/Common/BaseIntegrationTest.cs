using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFlow.Persistence;

namespace WebApi.IntegrationTests.Common;

public abstract class BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory = new();
    private IServiceScope? _scope;
    protected ISender Sender { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        _scope = _factory.Services.CreateScope();
        
        // Migrate database - in-memory SQLite creates fresh db per scope
        var dbContext = _scope.ServiceProvider.GetRequiredService<PetFlowDbContext>();
        dbContext.Database.Migrate();
        
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [TearDown]
    public void TearDown()
    {
        _scope?.Dispose();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _factory.Dispose();
    }
}