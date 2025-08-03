using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IEventRepository
{
    Task<Event> CreateAsync(Event petEvent, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Event petEvent, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetEventsByPetIdAsync(int petId, CancellationToken cancellationToken = default);
    Task AddPetsToEventAsync(IList<PetEvent> petEvents, CancellationToken cancellationToken = default);
}