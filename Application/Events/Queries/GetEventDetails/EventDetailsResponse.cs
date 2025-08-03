using Application.Events.Common;

namespace Application.Events.Queries.GetEventDetails;

public class EventDetailsResponse : EventResponse
{
    public DateTime ModifiedAt { get; set; }
    public IEnumerable<AssignedPetDto> AssignedPets { get; set; } = [];
}