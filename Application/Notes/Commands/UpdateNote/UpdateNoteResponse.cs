using System.Text.Json.Serialization;
using Application.Notes.Common;

namespace Application.Notes.Commands.UpdateNote;

public record UpdateNoteResponse : NoteResponse
{
    [JsonPropertyName("content")]
    public string Content { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
}