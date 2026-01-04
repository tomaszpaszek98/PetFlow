using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly PetFlowFlowDbContext _dbContext;

    public EventRepository(PetFlowFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event> CreateAsync(Event petEvent, CancellationToken cancellationToken = default)
    {
        await _dbContext.Events.AddAsync(petEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return petEvent;
    }
    
    public async Task<Event?> GetByIdWithPetEventsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Events
            .AsNoTracking()
            .Include(e => e.PetEvents)
            .ThenInclude(pe => pe.Pet)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Event?> GetByIdWithPetEventsTrackedAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Events
            .Include(e => e.PetEvents)
            .ThenInclude(pe => pe.Pet)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Events
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Event petEvent, CancellationToken cancellationToken = default)
    {
        _dbContext.Events.Update(petEvent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var removed = await _dbContext.Events
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return removed > 0;
    }
}