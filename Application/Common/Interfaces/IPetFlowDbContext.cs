using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IPetFlowDbContext
{
    DbSet<Event> Events { get; set; }
    DbSet<MedicalNote> MedicalNotes { get; set; }
    DbSet<Note> Notes { get; set; }
    DbSet<Pet> Pets { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}