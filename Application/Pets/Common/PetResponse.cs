using System.Text.Json.Serialization;

namespace Application.Pets.Common;

public record PetResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("species")]
    public string Species { get; init; }
    [JsonPropertyName("breed")]
    public string Breed { get; init; }
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; init; }
    [JsonPropertyName("photoUrl")]
    public string? PhotoUrl { get; init; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime? ModifiedAt { get; init; }
}