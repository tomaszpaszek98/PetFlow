using Domain.Common;

namespace Domain.Entities;

public class Event : AuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateOfEvent { get; set; }
    public bool Reminder { get; set; }
    public int PetId { get; set; }
    public Pet Pet { get; set; }
}