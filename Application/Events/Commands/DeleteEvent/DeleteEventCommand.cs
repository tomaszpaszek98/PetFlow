using MediatR;

namespace Application.Events.Commands.DeleteEvent;

public record DeleteEventCommand : IRequest
{
    public int EventId { get; init; }
}