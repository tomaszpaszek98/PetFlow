using MediatR;

namespace Application.Events.Commands.DeleteEvent;

public class DeleteEventCommand : IRequest
{
    public int EventId { get; set; }
}