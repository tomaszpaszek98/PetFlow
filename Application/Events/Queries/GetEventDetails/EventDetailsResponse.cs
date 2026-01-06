using System.Text.Json.Serialization;
using Application.Events.Common;

namespace Application.Events.Queries.GetEventDetails;

public record EventDetailsResponse : EventResponse
{
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
    [JsonPropertyName("assignedPets")]
    public IEnumerable<AssignedPetDto> AssignedPets { get; init; } = [];
}