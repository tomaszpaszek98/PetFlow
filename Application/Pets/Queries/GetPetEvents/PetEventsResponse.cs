using System.Text.Json.Serialization;

namespace Application.Pets.Queries.GetPetEvents;

public record PetEventsResponse
{
    [JsonPropertyName("items")]
    public IEnumerable<PetEventResponse> Items { get; init; } = [];
}

public record PetEventResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("dateOfEvent")]
    public DateTime DateOfEvent { get; init; }
    [JsonPropertyName("reminder")]
    public bool Reminder { get; init; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime? ModifiedAt { get; init; }
}