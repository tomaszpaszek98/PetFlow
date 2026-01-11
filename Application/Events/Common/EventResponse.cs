using System.Text.Json.Serialization;

namespace Application.Events.Common;

public record EventResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("dateOfEvent")]
    public DateTime DateOfEvent { get; init; }
    [JsonPropertyName("reminder")]
    public bool Reminder { get; init; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }
}