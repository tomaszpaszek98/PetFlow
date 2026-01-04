using System.Text.Json.Serialization;

namespace PetFlow.Requests.Event;

public record UpdateEventRequest(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("dateOfEvent")] DateTime DateOfEvent,
    [property: JsonPropertyName("reminder")] bool Reminder,
    [property: JsonPropertyName("petToAssignIds")] IEnumerable<int> PetToAssignIds
);
