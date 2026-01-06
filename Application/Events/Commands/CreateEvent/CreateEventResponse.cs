using System.Text.Json.Serialization;
using Application.Events.Common;

namespace Application.Events.Commands.CreateEvent;

public record CreateEventResponse : EventResponse
{
    [JsonPropertyName("assignedPets")]
    public IEnumerable<AssignedPetDto>? AssignedPets { get; init; }
}