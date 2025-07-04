using Domain.Common;

namespace Domain.Entities;

public class MedicalNotes : AuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int PetId { get; set; }
    public Pet Pet { get; set; }
}