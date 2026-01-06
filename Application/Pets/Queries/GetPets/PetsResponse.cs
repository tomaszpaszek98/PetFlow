using System.Text.Json.Serialization;
using Application.Pets.Common;

namespace Application.Pets.Queries.GetPets;

public record PetsResponse
{
    [JsonPropertyName("items")]
    public IEnumerable<PetResponse> Items { get; init; } = [];
}

