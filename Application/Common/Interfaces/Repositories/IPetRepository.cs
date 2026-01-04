using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IPetRepository
{
    Task<Pet> CreateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<Pet?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Pet?> GetByIdWithUpcomingEventAsync(int id, CancellationToken cancellationToken = default);
    Task<Pet?> GetByIdWithEventsAsync(int id, CancellationToken cancellationToken = default);
    Task<IList<Pet>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IList<Pet>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}