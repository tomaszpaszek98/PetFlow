using MediatR;

namespace Application.Events.Queries.GetEvents;

public record GetEventsQuery : IRequest<EventsResponse>;