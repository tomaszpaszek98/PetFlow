namespace Application.Events.Commands.CreateEvent;

public class EventResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateOfEvent { get; set; }
    public bool Reminder { get; set; }
    public IEnumerable<int>? AssignedPetIds { get; set; }
    public IEnumerable<int>? MissingPetIds { get; set; }
}