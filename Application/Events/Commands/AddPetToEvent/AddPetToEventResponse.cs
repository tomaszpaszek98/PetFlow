namespace Application.Events.Commands.AddPetToEvent;

public class AddPetToEventResponse
{
    public int EventId { get; set; }
    public int PetId { get; set; }
    public DateTime AssociatedAt { get; set; }
}