using MediatR;

namespace Application.Events.Queries.GetEventDetails;

public class GetEventDetailsQuery : IRequest<EventDetailsResponse>
{
    public int EventId { get; set; }
}