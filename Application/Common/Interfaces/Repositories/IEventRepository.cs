using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IEventRepository
{
    Task<Event> CreateAsync(Event petEvent, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdWithPetEventsAsync(int id, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdWithPetEventsTrackedAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Event petEvent, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}