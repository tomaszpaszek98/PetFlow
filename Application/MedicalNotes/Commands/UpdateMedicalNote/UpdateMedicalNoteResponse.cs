using System.Text.Json.Serialization;
using Application.MedicalNotes.Common;

namespace Application.MedicalNotes.Commands.UpdateMedicalNote;

public record UpdateMedicalNoteResponse : MedicalNoteResponse
{
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
}