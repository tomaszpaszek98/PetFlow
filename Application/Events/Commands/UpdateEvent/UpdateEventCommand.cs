using MediatR;

namespace Application.Events.Commands.UpdateEvent;

public class UpdateEventCommand : IRequest<UpdateEventResponse>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateOfEvent { get; set; }
    public bool Reminder { get; set; }
    public IEnumerable<int> PetToAssignIds { get; set; }
}