using System.Text.Json.Serialization;
using Application.Pets.Common;
using MediatR;

namespace Application.Pets.Commands.CreatePet;

public record CreatePetCommand : IRequest<PetResponse>
{
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("species")]
    public string Species { get; init; }
    [JsonPropertyName("breed")]
    public string Breed { get; init; }
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; init; }
}