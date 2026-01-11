using Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Queries.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, EventsResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<GetEventsQueryHandler> _logger;

    public GetEventsQueryHandler(IEventRepository eventRepository, ILogger<GetEventsQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<EventsResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetEventsQuery");
        
        var events = (await _eventRepository.GetAllAsync(cancellationToken)).ToList();

        _logger.LogInformation("Retrieved {Count} events successfully", events.Count);
        return events.MapToResponse();
    }
}
