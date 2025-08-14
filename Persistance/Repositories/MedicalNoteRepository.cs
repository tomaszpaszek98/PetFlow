using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace PetFlow.Persistance.Repositories;

public class MedicalNoteRepository : IMedicalNoteRepository
{
    private readonly List<MedicalNote> _medicalNotes = new();

    public Task<MedicalNote> CreateAsync(MedicalNote medicalNote, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _medicalNotes.Add(medicalNote);
        return Task.FromResult(medicalNote);
    }

    public Task<MedicalNote?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var medicalNote = _medicalNotes.SingleOrDefault(x => x.Id == id);
        
        return Task.FromResult(medicalNote);
    }

    public Task<IEnumerable<MedicalNote>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(_medicalNotes.AsEnumerable());
    }

    public Task UpdateAsync(MedicalNote medicalNote, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var noteIndex = _medicalNotes.FindIndex(x => x.Id == medicalNote.Id);
        if (noteIndex == -1)
        {
            throw new Exception($"Medical note with id {medicalNote.Id} not found");
        }
        _medicalNotes[noteIndex] = medicalNote;
        
        return Task.CompletedTask;
    }

    public Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removedCount = _medicalNotes.RemoveAll(x => x.Id == id);
        
        return Task.FromResult(removedCount > 0);
    }
    
    public Task<IEnumerable<MedicalNote>> GetAllByPetIdAsync(int petId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var medicalNotes = _medicalNotes.Where(x => x.PetId == petId);
        
        return Task.FromResult(medicalNotes);
    }
}

