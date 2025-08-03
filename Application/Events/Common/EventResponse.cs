namespace Application.Events.Common;

public class EventResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateOfEvent { get; set; }
    public bool Reminder { get; set; }
    public DateTime CreatedAt { get; set; }
}