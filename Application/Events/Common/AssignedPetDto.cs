using System.Text.Json.Serialization;

namespace Application.Events.Common;

public record AssignedPetDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("photoUrl")]
    public string? PhotoUrl { get; init; }
}