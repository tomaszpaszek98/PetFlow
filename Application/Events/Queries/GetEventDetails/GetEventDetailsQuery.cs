using MediatR;

namespace Application.Events.Queries.GetEventDetails;

public record GetEventDetailsQuery : IRequest<EventDetailsResponse>
{
    public int EventId { get; init; }
}