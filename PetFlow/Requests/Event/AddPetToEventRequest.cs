using System.Text.Json.Serialization;

namespace PetFlow.Requests.Event;

public record AddPetToEventRequest(
    [property: JsonPropertyName("petId")] int PetId
);
