using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryHandler : IRequestHandler<GetEventDetailsQuery, EventDetailsResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<GetEventDetailsQueryHandler> _logger;

    public GetEventDetailsQueryHandler(IEventRepository eventRepository, ILogger<GetEventDetailsQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<EventDetailsResponse> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetEventDetailsQuery for EventId: {EventId}", request.EventId);
        
        var eventDetails = await _eventRepository.GetByIdWithPetEventsAsync(request.EventId, cancellationToken);

        if (eventDetails is null)
        {
            _logger.LogError("Event with ID {EventId} not found", request.EventId);
            throw new NotFoundException(nameof(Event), request.EventId);
        }

        _logger.LogInformation("Event details retrieved successfully for EventId: {EventId}", request.EventId);
        return eventDetails.MapToEventDetailsResponse();
    }
}
