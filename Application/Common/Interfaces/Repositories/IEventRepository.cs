using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IEventRepository
{
    Task<bool> CreateAsync(Event petEvent, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Event?> GetUpcomingEventForPetAsync(int petId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Event petEvent, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}