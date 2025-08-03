using MediatR;

namespace Application.Events.Commands.CreateEvent;

public class CreateEventCommand : IRequest<CreateEventResponse>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateOfEvent { get; set; }
    public bool Reminder { get; set; }
    public IEnumerable<int>? PetToAssignIds { get; set; }
}