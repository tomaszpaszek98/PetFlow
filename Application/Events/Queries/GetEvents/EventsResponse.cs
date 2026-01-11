using System.Text.Json.Serialization;

namespace Application.Events.Queries.GetEvents;

public record EventsResponse
{
    [JsonPropertyName("items")]
    public IEnumerable<EventResponseDto> Items { get; init; } = [];
}
