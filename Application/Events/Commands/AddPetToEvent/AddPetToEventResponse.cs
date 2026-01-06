using System.Text.Json.Serialization;

namespace Application.Events.Commands.AddPetToEvent;

public record AddPetToEventResponse
{
    [JsonPropertyName("eventId")]
    public int EventId { get; init; }
    [JsonPropertyName("petId")]
    public int PetId { get; init; }
    [JsonPropertyName("associatedAt")]
    public DateTime AssociatedAt { get; init; }
}