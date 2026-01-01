using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly PetFlowFlowDbContext _dbContext;

    public NoteRepository(PetFlowFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default)
    {
        await _dbContext.Notes.AddAsync(note, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return note;
    }

    public async Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notes
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<Note?> GetByIdWithPetAsync(int noteId, int petId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notes
            .AsNoTracking()
            .Include(n => n.Pet)
            .FirstOrDefaultAsync(n => n.Id == noteId && n.PetId == petId, cancellationToken);
    }

    public async Task UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        _dbContext.Notes.Update(note);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteByIdAsync(int id, int petId, CancellationToken cancellationToken = default)
    {
        var removed = await _dbContext.Notes
            .Where(n => n.Id == id && n.PetId == petId)
            .ExecuteDeleteAsync(cancellationToken);

        return removed > 0;
    }

    public async Task<IEnumerable<Note>> GetAllByPetIdAsync(int petId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notes
            .AsNoTracking()
            .Where(n => n.PetId == petId)
            .ToListAsync(cancellationToken);
    }
}

