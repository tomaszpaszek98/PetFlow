using System.Text.Json.Serialization;

namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public record MedicalNotesResponse
{
    [JsonPropertyName("items")]
    public IEnumerable<MedicalNoteResponseDto> Items { get; init; } = [];
}