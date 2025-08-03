using MediatR;

namespace Application.Events.Queries.GetEvents;

public class GetEventsQuery : IRequest<EventsResponse>
{
}
