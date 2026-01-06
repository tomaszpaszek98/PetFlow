using System.Text.Json.Serialization;

namespace Application.Notes.Queries.GetNotes;

public record NotesResponse
{
    [JsonPropertyName("items")] 
    public IEnumerable<NoteResponseDto> Items { get; init; } = [];
}