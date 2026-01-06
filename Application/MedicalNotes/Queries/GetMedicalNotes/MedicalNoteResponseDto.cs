using System.Text.Json.Serialization;

namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public record MedicalNoteResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
}