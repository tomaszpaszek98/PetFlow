using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Persistance.Repositories;

namespace PetFlow.Persistance.Repositories;

public class EventRepository : IEventRepository
{
    private readonly List<Event> _events = new();

    public Task<Event> CreateAsync(Event petEvent, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _events.Add(petEvent);
        return Task.FromResult(petEvent);
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
            .Where(e => e.PetEvents.Any(pe => pe.PetId == petId) && e.DateOfEvent > now)
            .OrderBy(e => e.DateOfEvent)
            .FirstOrDefault();
        return Task.FromResult(petEvent);
    }

    public Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_events.AsEnumerable());
    }

    public Task UpdateAsync(Event petEvent, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var eventIndex = _events.FindIndex(x => x.Id == petEvent.Id);
        if (eventIndex == -1)
        {
            throw new Exception($"Event with id {petEvent.Id} not found");
        }
        _events[eventIndex] = petEvent;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removedCount = _events.RemoveAll(x => x.Id == id);
        return Task.FromResult(removedCount > 0);
    }
    
    public Task<IEnumerable<Event>> GetEventsByPetIdAsync(int petId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var events = _events.Where(e => e.PetEvents.Any(pe => pe.PetId == petId));
        return Task.FromResult(events);
    }

    public Task AddPetsToEventAsync(IList<PetEvent> petEvents, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        foreach (var petEvent in petEvents)
        {
            var eventObj = _events.SingleOrDefault(e => e.Id == petEvent.EventId);
            if (eventObj != null)
            {
                if (eventObj.PetEvents == null)
                    eventObj.PetEvents = new List<PetEvent>();
                eventObj.PetEvents.Add(petEvent);
            }
        }
        return Task.CompletedTask;
    }

    public Task<bool> RemovePetFromEventAsync(int eventId, int petId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var eventEntity = _events.SingleOrDefault(e => e.Id == eventId);
        if (eventEntity == null || eventEntity.PetEvents == null)
        {
            return Task.FromResult(false);
        }
        
        var petEventToRemove = eventEntity.PetEvents.FirstOrDefault(pe => pe.PetId == petId);
        if (petEventToRemove == null)
        {
            return Task.FromResult(false);
        }
        
        var removed = eventEntity.PetEvents.Remove(petEventToRemove);
        return Task.FromResult(removed);
    }
}