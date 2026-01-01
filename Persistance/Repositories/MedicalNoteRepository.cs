using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence.Repositories;

public class MedicalNoteRepository : IMedicalNoteRepository
{
    private readonly PetFlowFlowDbContext _dbContext;

    public MedicalNoteRepository(PetFlowFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MedicalNote> CreateAsync(MedicalNote medicalNote, CancellationToken cancellationToken = default)
    {
        await _dbContext.MedicalNotes.AddAsync(medicalNote, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return medicalNote;
    }

    public async Task<MedicalNote?> GetByIdWithPetAsync(int medicalNoteId, int petId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MedicalNotes
            .AsNoTracking()
            .Include(m => m.Pet)
            .FirstOrDefaultAsync(m => m.Id == medicalNoteId && m.PetId == petId, cancellationToken);
    }

    public async Task UpdateAsync(MedicalNote medicalNote, CancellationToken cancellationToken = default)
    {
        _dbContext.MedicalNotes.Update(medicalNote);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteByIdAsync(int medicalNoteId, int petId, CancellationToken cancellationToken = default)
    {
        var removed = await _dbContext.MedicalNotes
            .Where(m => m.Id == medicalNoteId && m.PetId == petId)
            .ExecuteDeleteAsync(cancellationToken);

        return removed > 0;
    }
    
    public async Task<IEnumerable<MedicalNote>> GetAllByPetIdAsync(int petId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MedicalNotes
            .AsNoTracking()
            .Where(m => m.PetId == petId)
            .ToListAsync(cancellationToken);
    }
}
