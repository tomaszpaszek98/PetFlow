using System.Text.Json.Serialization;

namespace PetFlow.Requests.MedicalNote;

public record UpdateMedicalNoteRequest(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description
);
