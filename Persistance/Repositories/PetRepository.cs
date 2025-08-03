using Domain.Entities;
using Persistance.Repositories;

namespace PetFlow.Persistance.Repositories;

public class PetRepository : IPetRepository
{
    private readonly List<Pet> _pets = new();

    public Task<Pet> CreateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _pets.Add(pet);
        return Task.FromResult(pet);
    }

    public Task<Pet?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var pet = _pets.SingleOrDefault(x => x.Id == id);
        
        return Task.FromResult(pet);
    }

    public Task<IEnumerable<Pet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(_pets.AsEnumerable());
    }

    public Task UpdateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var petIndex = _pets.FindIndex(x => x.Id == pet.Id);
        if (petIndex == -1)
        {
            throw new Exception($"Pet with id {pet.Id} not found");
        }
        _pets[petIndex] = pet;
        
        return Task.CompletedTask;
    }

    public Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removedCount = _pets.RemoveAll(x => x.Id == id);
        
        return Task.FromResult(removedCount > 0);
    }
    

    public Task<Event?> GetUpcomingEventForPetAsync(int petId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var pet = _pets.SingleOrDefault(x => x.Id == petId);
        if (pet == null || pet.PetEvents == null)
            return Task.FromResult<Event?>(null);
        var now = DateTime.Now;
        var upcomingEvent = pet.PetEvents
            .Select(pe => pe.Event)
            .Where(e => e != null && e.DateOfEvent > now)
            .OrderBy(e => e.DateOfEvent)
            .FirstOrDefault();
        
        return Task.FromResult(upcomingEvent);
    }

    public Task<IEnumerable<Pet>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var idSet = ids.ToHashSet();
        var pets = _pets.Where(x => idSet.Contains(x.Id));
        
        return Task.FromResult(pets);
    }
}
