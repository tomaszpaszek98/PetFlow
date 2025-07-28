using Domain.Entities;

namespace Persistance.Repositories;

public interface IPetRepository
{
    Task<bool> CreateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<Pet?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pet>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>>  GetEventsByPetIdAsync(int petId, CancellationToken cancellationToken = default);
}