using Domain.Entities;

namespace Persistance.Repositories;

public interface IPetRepository
{
    Task<Pet> CreateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<Pet?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pet>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pet>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}