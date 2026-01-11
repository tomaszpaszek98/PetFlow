using System.Text.Json.Serialization;

namespace Application.MedicalNotes.Common;

public record MedicalNoteResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }
}