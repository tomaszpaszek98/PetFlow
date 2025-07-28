using Domain.Entities;
using Persistance.Repositories;

namespace PetFlow.Persistance.Repositories;

public class PetRepository : IPetRepository
{
    private readonly List<Pet> _pets = new();

    public Task<bool> CreateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _pets.Add(pet);
        return Task.FromResult(true);
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

    public Task<bool> UpdateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var petIndex = _pets.FindIndex(x => x.Id == pet.Id);
        if (petIndex == -1)
        {
            return Task.FromResult(false);
        }
        _pets[petIndex] = pet;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removedCount = _pets.RemoveAll(x => x.Id == id);
        var petRemoved = removedCount > 0;
        return Task.FromResult(petRemoved);
    }

    public Task<IEnumerable<Event>> GetEventsByPetIdAsync(int petId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var pet = _pets.SingleOrDefault(x => x.Id == petId);
        if (pet == null || pet.PetEvents == null)
            return Task.FromResult(Enumerable.Empty<Event>());
        return Task.FromResult(pet.PetEvents
            .Where(x => x.Event != null)
            .Select(x => x.Event));
    }
}
