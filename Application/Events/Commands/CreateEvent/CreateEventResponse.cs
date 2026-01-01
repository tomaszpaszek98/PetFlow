using Application.Events.Common;

namespace Application.Events.Commands.CreateEvent;

public class CreateEventResponse : EventResponse
{
    public IEnumerable<AssignedPetDto>? AssignedPets { get; set; }
}