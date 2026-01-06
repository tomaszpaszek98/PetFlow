using System.Text.Json.Serialization;
using Application.Notes.Common;

namespace Application.Notes.Commands.CreateNote;

public record CreateNoteResponse : NoteResponse
{
    [JsonPropertyName("content")]
    public string Content { get; init; }
}