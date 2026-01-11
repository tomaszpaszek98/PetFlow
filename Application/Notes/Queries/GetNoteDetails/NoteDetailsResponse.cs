using System.Text.Json.Serialization;
using Application.Notes.Common;

namespace Application.Notes.Queries.GetNoteDetails;

public record NoteDetailsResponse : NoteResponse
{
    [JsonPropertyName("content")]
    public string Content { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
}
