using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence.Repositories;

public class PetRepository : IPetRepository
{
    private readonly PetFlowFlowDbContext _dbContext;

    public PetRepository(PetFlowFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Pet> CreateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        await _dbContext.Pets.AddAsync(pet, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return pet;
    }

    public async Task<Pet?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pets
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IList<Pet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pets
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        _dbContext.Pets.Update(pet);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var removed = await _dbContext.Pets
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return removed > 0;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pets
            .AsNoTracking()
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IList<Pet>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var idList = ids.ToHashSet();
        
        if (!idList.Any())
            return [];
        
        return await _dbContext.Pets
            .Where(x => idList.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Pet?> GetByIdWithUpcomingEventAsync(int id, CancellationToken cancellationToken = default)
    {
        var pet = await _dbContext.Pets
            .AsNoTracking()
            .Include(p => p.Events)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (pet == null)
            return null;
        
        // We add filtering and ordering logic here to only return the next upcoming event
        // With SQL Server it should be possible to do it in query (with Include)
        // but with SQLite it is not supported, so we do it in memory
        pet.Events = pet.Events
            .Where(e => e.DateOfEvent >= DateTime.UtcNow)
            .OrderBy(e => e.DateOfEvent)
            .Take(1)
            .ToList();

        return pet;
    }

    public async Task<Pet?> GetByIdWithEventsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pets
            .AsNoTracking()
            .Include(p => p.Events)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}