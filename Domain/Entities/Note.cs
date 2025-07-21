using Domain.Common;

namespace Domain.Entities;

public class Note : AuditableEntity
{
    public string Content { get; set; }
    public NoteType Type { get; set; }
    public int PetId { get; set; }
    public Pet Pet { get; set; }
}

public enum NoteType
{
    Behaviour,
    Mood,
    Symptom,
    General
}