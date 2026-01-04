using Application.Pets.Commands.CreatePet;
using Application.Pets.Common;
using Application.Pets.Queries.GetPetDetails;
using Application.Pets.Queries.GetPetEvents;
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
            PhotoUrl = pet.PhotoUrl,
            CreatedAt = pet.Created,
            ModifiedAt = pet.Modified ?? pet.Created
        };
    }

    public static PetDetailsResponse MapToPetDetailsResponse(this Pet pet)
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
            ModifiedAt = pet.Modified ?? pet.Created,
            UpcomingEvent = MapUpcomingEvent(pet.Events.FirstOrDefault())
        };
    }
    
    public static PetEventsResponse MapToResponse(this IEnumerable<Event> petEvents)
    {
        return new PetEventsResponse
        {
            Items = petEvents.Select(MapToResponse)
        };
    }

    private static PetEventResponse MapToResponse(this Event petEvent)
    {
        return new PetEventResponse
        {
            Id = petEvent.Id,
            Title = petEvent.Title,
            DateOfEvent = petEvent.DateOfEvent,
            Reminder = petEvent.Reminder,
            CreatedAt = petEvent.Created,
            ModifiedAt = petEvent.Modified ?? petEvent.Created
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

    public static PetsResponse MapToResponse(this IEnumerable<Pet> pets)
    {
        return new PetsResponse
        {
            Items = pets.Select(MapToResponse).ToList()
        };
    }
}