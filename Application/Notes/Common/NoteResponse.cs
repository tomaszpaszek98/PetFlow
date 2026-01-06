using System.Text.Json.Serialization;
using Domain.Enums;

namespace Application.Notes.Common;

public record NoteResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("type")]
    public NoteType Type { get; init; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }
}