using System.Text.Json.Serialization;
using Application.Events.Common;

namespace Application.Events.Queries.GetEvents;

public record EventResponseDto : EventResponse
{
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; init; }
}
