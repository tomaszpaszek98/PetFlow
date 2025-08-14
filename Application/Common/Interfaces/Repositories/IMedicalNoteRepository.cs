using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IMedicalNoteRepository
{
    Task<MedicalNote> CreateAsync(MedicalNote medicalNote, CancellationToken cancellationToken = default);
    Task<MedicalNote?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(MedicalNote medicalNote, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalNote>> GetAllByPetIdAsync(int petId, CancellationToken cancellationToken = default);
}

