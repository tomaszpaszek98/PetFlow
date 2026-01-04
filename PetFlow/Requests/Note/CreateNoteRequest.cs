using Domain.Enums;
using System.Text.Json.Serialization;

namespace PetFlow.Requests.Note;

public record CreateNoteRequest(
    [property: JsonPropertyName("content")] string Content,
    // Exceptional use of domain's enum here.
    [property: JsonPropertyName("type")] NoteType Type
);
