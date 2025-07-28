using Domain.Common;

namespace Domain.Entities;

public class Pet : AuditableEntity
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhotoUrl { get; set; }
    public ICollection<MedicalNote> MedicalNotes { get; set; }
    public ICollection<Note> Notes { get; set; }
    public ICollection<PetEvent> PetEvents { get; set; }
}