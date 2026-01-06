using System.Text.Json.Serialization;
using Application.MedicalNotes.Common;

namespace Application.MedicalNotes.Queries.GetMedicalNoteDetails;

public record MedicalNoteDetailsResponse : MedicalNoteResponse
{
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
}