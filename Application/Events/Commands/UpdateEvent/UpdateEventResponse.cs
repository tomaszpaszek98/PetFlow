using System.Text.Json.Serialization;
using Application.Events.Common;

namespace Application.Events.Commands.UpdateEvent;

public record UpdateEventResponse : EventResponse
{
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
    [JsonPropertyName("assignedPets")]
    public IEnumerable<AssignedPetDto> AssignedPets { get; init; } = [];
}
