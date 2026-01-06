using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence;

public class PetDbContextFactory : DesignTimeDbContextFactoryBase<PetFlowDbContext>
{
    protected override PetFlowDbContext CreateNewInstance(DbContextOptions<PetFlowDbContext> options)
    {
        return new PetFlowDbContext(options);
    }
}