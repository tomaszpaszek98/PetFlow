using System.Text.Json.Serialization;
using Application.Pets.Common;

namespace Application.Pets.Queries.GetPetDetails;

public record PetDetailsResponse : PetResponse
{
    [JsonPropertyName("upcomingEvent")]
    public UpcomingEventResponse? UpcomingEvent { get; init; }
}

public record UpcomingEventResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("eventDate")]
    public DateTime EventDate { get; init; }
}