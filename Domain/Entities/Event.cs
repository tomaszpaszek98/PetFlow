using Domain.Common;

namespace Domain.Entities;

// Pet prefix is used just to avoid conflicts with .net Event type. :)
public class PetEvent : AuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateOfEvent { get; set; }
    public bool Reminder { get; set; }
    public int PetId { get; set; }
    public Pet Pet { get; set; }
}