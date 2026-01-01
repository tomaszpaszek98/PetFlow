using Application.Events.Commands.CreateEvent;
using Application.Events.Commands.UpdateEvent;
using Application.Events.Common;
using Application.Events.Queries.GetEventDetails;
using Application.Events.Queries.GetEvents;
using Domain.Entities;

namespace Application.Events;

public static class MappingExtensions
{
    public static Event MapToEvent(this CreateEventCommand request, ICollection<Pet> pets)
    {
        return new Event
        {
            Title = request.Title,
            Description = request.Description,
            DateOfEvent = request.DateOfEvent,
            Reminder = request.Reminder,
            Pets = pets
        };
    }
    
    public static CreateEventResponse MapToResponse(this Event createdEvent, IList<Pet> assignedPets)
    {
        return new CreateEventResponse
        {
            Id = createdEvent.Id,
            Title = createdEvent.Title,
            Description = createdEvent.Description,
            DateOfEvent = createdEvent.DateOfEvent,
            Reminder = createdEvent.Reminder,
            AssignedPets = assignedPets.Select(MapToAssignedPetDto)
        };
    }

    public static EventDetailsResponse MapToEventDetailsResponse(this Event eventDetails)
    {
        return new EventDetailsResponse
        {
            Id = eventDetails.Id,
            Title = eventDetails.Title,
            Description = eventDetails.Description,
            DateOfEvent = eventDetails.DateOfEvent,
            Reminder = eventDetails.Reminder,
            AssignedPets = eventDetails.Pets.Select(MapToAssignedPetDto)
        };
    }

    public static EventResponseDto MapToResponseDto(this Event eventDetails)
    {
        return new EventResponseDto
        {
            Id = eventDetails.Id,
            Title = eventDetails.Title,
            Description = eventDetails.Description,
            DateOfEvent = eventDetails.DateOfEvent,
            Reminder = eventDetails.Reminder,
            CreatedAt = eventDetails.Created,
            ModifiedAt = eventDetails.Modified ?? eventDetails.Created
        };
    }
    
    public static UpdateEventResponse MapToUpdateResponse(this Event updatedEvent, IList<Pet> assignedPets)
    {
        return new UpdateEventResponse
        {
            Id = updatedEvent.Id,
            Title = updatedEvent.Title,
            Description = updatedEvent.Description,
            DateOfEvent = updatedEvent.DateOfEvent,
            Reminder = updatedEvent.Reminder,
            CreatedAt = updatedEvent.Created,
            ModifiedAt = updatedEvent.Modified ?? updatedEvent.Created,
            AssignedPets = assignedPets.Select(MapToAssignedPetDto)
        };
    }
    
    private static AssignedPetDto MapToAssignedPetDto(this Pet pet)
    {
        return new AssignedPetDto
        {
            Id = pet.Id,
            Name = pet.Name,
            PhotoUrl = pet.PhotoUrl
        };
    }

    public static EventsResponse MapToResponse(this IEnumerable<Event> events)
    {
        return new EventsResponse
        {
            Items = events.Select(MapToResponseDto).ToList()
        };
    }
}