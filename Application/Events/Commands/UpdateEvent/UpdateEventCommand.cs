using System.Text.Json.Serialization;
using MediatR;

namespace Application.Events.Commands.UpdateEvent;

public record UpdateEventCommand : IRequest<UpdateEventResponse>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("dateOfEvent")]
    public DateTime DateOfEvent { get; init; }
    [JsonPropertyName("reminder")]
    public bool Reminder { get; init; }
    [JsonPropertyName("assignedPetsIds")]
    public IEnumerable<int> AssignedPetsIds { get; init; }
}