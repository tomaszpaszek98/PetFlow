using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Persistance.Repositories;

namespace PetFlow.Persistance.Repositories;

public class EventRepository : IEventRepository
{
    private readonly List<Event> _events = new();

    public Task<bool> CreateAsync(Event pet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _events.Add(pet);
        return Task.FromResult(true);
    }

    public Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var petEvent = _events.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(petEvent);
    }

    public Task<Event?> GetUpcomingEventForPetAsync(int petId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var now = DateTime.Now;
        var petEvent = _events
            .Where(x => x.PetId == petId && x.DateOfEvent > now)
            .OrderBy(x => x.DateOfEvent)
            .FirstOrDefault();
        return Task.FromResult(petEvent);
    }

    public Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_events.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Event petEvent, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var eventIndex = _events.FindIndex(x => x.Id == petEvent.Id);
        if (eventIndex == -1)
        {
            return Task.FromResult(false);
        }
        _events[eventIndex] = petEvent;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removedCount = _events.RemoveAll(x => x.Id == id);
        var eventRemoved = removedCount > 0;
        return Task.FromResult(eventRemoved);
    }
}