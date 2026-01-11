using System.Text.Json.Serialization;
using MediatR;

namespace Application.Events.Commands.CreateEvent;

public record CreateEventCommand : IRequest<CreateEventResponse>
{
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("dateOfEvent")]
    public DateTime DateOfEvent { get; init; }
    [JsonPropertyName("reminder")]
    public bool Reminder { get; init; }
    [JsonPropertyName("petToAssignIds")]
    public IEnumerable<int>? PetToAssignIds { get; init; }
}