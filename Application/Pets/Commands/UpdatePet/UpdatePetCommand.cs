using Application.Pets.Common;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace Application.Pets.Commands.UpdatePet;

public record UpdatePetCommand : IRequest<PetResponse>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("species")]
    public string Species { get; init; }
    [JsonPropertyName("breed")]
    public string Breed { get; init; }
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; init; }
}

