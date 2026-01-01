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
    public ICollection<MedicalNote> MedicalNotes { get; set; } = new List<MedicalNote>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}