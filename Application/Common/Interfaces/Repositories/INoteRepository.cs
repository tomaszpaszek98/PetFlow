using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface INoteRepository
{
    Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default);
    Task<Note?> GetByIdWithPetAsync(int noteId, int petId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, int petId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Note>> GetAllByPetIdAsync(int petId, CancellationToken cancellationToken = default);
}
