using Domain.Common;

namespace Domain.Entities;

public class PetEvent : AuditableEntity
{
    public int PetId { get; set; }
    public Pet Pet { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
}

