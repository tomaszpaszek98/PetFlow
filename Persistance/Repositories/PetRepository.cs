using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence.Repositories;

public class PetRepository : IPetRepository
{
    private readonly IPetFlowDbContext _dbContext;

    public PetRepository(IPetFlowDbContext dbContext)
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

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
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
        return await _dbContext.Pets
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new Pet
            {
                Id = p.Id,
                UserId = p.UserId,
                Name = p.Name,
                Species = p.Species,
                Breed = p.Breed,
                DateOfBirth = p.DateOfBirth,
                PhotoUrl = p.PhotoUrl,
                Created = p.Created,
                CreatedBy = p.CreatedBy,
                Modified = p.Modified,
                ModifiedBy = p.ModifiedBy,
                StatusId = p.StatusId,
                Inactivated = p.Inactivated,
                InactivatedBy = p.InactivatedBy,
                Events = p.Events
                    .Where(e => e.DateOfEvent >= DateTime.UtcNow)
                    .OrderBy(e => e.DateOfEvent)
                    .Take(1)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Pet?> GetByIdWithEventsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pets
            .AsNoTracking()
            .Include(p => p.Events)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}