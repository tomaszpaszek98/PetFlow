using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence;

public class PetDbContextFactory : DesignTimeDbContextFactoryBase<PetFlowFlowDbContext>
{
    protected override PetFlowFlowDbContext CreateNewInstance(DbContextOptions<PetFlowFlowDbContext> options)
    {
        return new PetFlowFlowDbContext(options);
    }
}