using Application.Common.Interfaces.Repositories;
using Application.Events;
using Application.Pets;
using Domain.Entities;
using MediatR;

namespace Application.Events.Queries.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, EventsResponse>
{
    private readonly IEventRepository _eventRepository;

    public GetEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventsResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);

        return events.MapToResponse();
    }
}
