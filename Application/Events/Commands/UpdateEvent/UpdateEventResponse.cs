using Application.Events.Common;

namespace Application.Events.Commands.UpdateEvent;

public class UpdateEventResponse : EventResponse
{
    public DateTime ModifiedAt { get; set; }
    public IEnumerable<AssignedPetDto> AssignedPets { get; set; } = new List<AssignedPetDto>();
}
