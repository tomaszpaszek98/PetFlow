namespace Application.Events.Queries.GetEvents;

public class EventsResponse
{
    public IEnumerable<EventResponseDto> Items { get; set; } = new List<EventResponseDto>();
}
