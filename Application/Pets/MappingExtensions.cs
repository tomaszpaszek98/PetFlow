using Application.Pets.Commands.CreatePet;
using Application.Pets.Commands.UpdatePet;
using Application.Pets.Common;
using Application.Pets.Queries.GetPetDetails;
using Application.Pets.Queries.GetPets;
using Domain.Entities;

namespace Application.Pets;

public static class MappingExtensions
{
    public static Pet MapToPet(this CreatePetCommand command)
    {
        return new Pet
        {
            Name = command.Name,
            Species = command.Species,
            Breed = command.Breed,
            DateOfBirth = command.DateOfBirth
        };
    }

    public static PetResponse MapToResponse(this Pet pet)
    {
        return new PetResponse
        {
            Id = pet.Id,
            Name = pet.Name,
            Species = pet.Species,
            Breed = pet.Breed,
            DateOfBirth = pet.DateOfBirth,
            PhotoUrl = pet.PhotoUrl
        };
    }

    public static PetDetailsResponse MapToPetDetailsResponse(this Pet pet, Event? petEvent)
    {
        return new PetDetailsResponse
        {
            Id = pet.Id,
            Name = pet.Name,
            Species = pet.Species,
            Breed = pet.Breed,
            DateOfBirth = pet.DateOfBirth,
            PhotoUrl = pet.PhotoUrl,
            CreatedAt = pet.Created,
            ModifiedAt = pet.Modified,
            UpcomingEvent = MapUpcomingEvent(petEvent)
        };
    }
    
    public static PetsResponse MapToResponse(this IEnumerable<Pet> pets)
    {
        return new PetsResponse
        {
            Items = pets.Select(MapToResponse)
        };
    }

    private static UpcomingEventResponse? MapUpcomingEvent(Event? petEvent)
    {
        if (petEvent is null)
            return null;
        
        return new UpcomingEventResponse
        {
            Id = petEvent.Id,
            Title = petEvent.Title,
            EventDate = petEvent.DateOfEvent
        };
    }
}