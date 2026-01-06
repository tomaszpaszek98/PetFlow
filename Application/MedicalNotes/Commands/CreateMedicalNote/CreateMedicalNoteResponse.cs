using System.Text.Json.Serialization;
using Application.MedicalNotes.Common;

namespace Application.MedicalNotes.Commands.CreateMedicalNote;

public record CreateMedicalNoteResponse : MedicalNoteResponse
{
    [JsonPropertyName("description")]
    public string Description { get; init; }
}