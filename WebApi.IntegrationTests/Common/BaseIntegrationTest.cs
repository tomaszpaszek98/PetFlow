using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFlow.Persistence;

namespace WebApi.IntegrationTests.Common;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public abstract class BaseIntegrationTest
{
    private IntegrationTestWebAppFactory? _factory;
    private IServiceScope? _scope;
    protected ISender Sender { get; private set; } = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        _factory = new IntegrationTestWebAppFactory();
        await _factory.InitializeAsync();
        _scope = _factory.Services.CreateScope();
        
        var dbContext = _scope.ServiceProvider.GetRequiredService<PetFlowDbContext>();
        await dbContext.Database.MigrateAsync();
        
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _scope?.Dispose();
        
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
    }
}