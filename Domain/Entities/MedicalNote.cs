using Domain.Common;

namespace Domain.Entities;

public class MedicalNote : AuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int PetId { get; set; }
    public Pet Pet { get; set; }
}