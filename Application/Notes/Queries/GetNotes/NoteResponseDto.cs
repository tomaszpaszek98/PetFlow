using System.Text.Json.Serialization;
using Application.Notes.Common;

namespace Application.Notes.Queries.GetNotes;

public record NoteResponseDto : NoteResponse
{
    [JsonPropertyName("shortContent")]
    public string ShortContent { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
}