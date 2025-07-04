using Domain.Common;

namespace Domain.Entities;

public class Pets : AuditableEntity
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhotoUrl { get; set; }
    public ICollection<MedicalNotes> MedicalNotes { get; set; }
    public ICollection<Notes> Notes { get; set; }
    public ICollection<Events> Events { get; set; }
}