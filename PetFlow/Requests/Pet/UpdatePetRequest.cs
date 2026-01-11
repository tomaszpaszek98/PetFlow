using System.Text.Json.Serialization;
using Application.Pets.Commands.UpdatePet;

namespace PetFlow.Requests.Pet;

public record UpdatePetRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("species")] string Species,
    [property: JsonPropertyName("breed")] string Breed,
    [property: JsonPropertyName("dateOfBirth")] DateTime DateOfBirth
);
