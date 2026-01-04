using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryHandler : IRequestHandler<GetEventDetailsQuery, EventDetailsResponse>
{
    private readonly IEventRepository _eventRepository;

    public GetEventDetailsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDetailsResponse> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
    {
        var eventDetails = await _eventRepository.GetByIdWithPetEventsAsync(request.EventId, cancellationToken);

        if (eventDetails is null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }

        return eventDetails.MapToEventDetailsResponse();
    }
}
