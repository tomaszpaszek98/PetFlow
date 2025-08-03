using Application.Events.Common;

namespace Application.Events.Queries.GetEvents;

public class EventResponseDto : EventResponse
{
    public DateTime ModifiedAt { get; set; }
}
