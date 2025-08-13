using Application.Events.Commands.CreateEvent;
using Application.Events.Commands.UpdateEvent;
using Application.Events.Common;
using Application.Events.Queries.GetEventDetails;
using Application.Events.Queries.GetEvents;
using Domain.Entities;

namespace Application.Events;

public static class MappingExtensions
{
    public static Event MapToEvent(this CreateEventCommand request)
    {
        return new Event
        {
            Title = request.Title,
            Description = request.Description,
            DateOfEvent = request.DateOfEvent,
            Reminder = request.Reminder
        };
    }
    
    public static CreateEventResponse MapToResponse(this Event createdEvent, IList<Pet> assignedPets, IList<int> notFoundPetIds)
    {
        return new CreateEventResponse
        {
            Id = createdEvent.Id,
            Title = createdEvent.Title,
            Description = createdEvent.Description,
            DateOfEvent = createdEvent.DateOfEvent,
            Reminder = createdEvent.Reminder,
            AssignedPets = assignedPets.Select(MapToAssignedPetDto),
            MissingPetIds = notFoundPetIds
        };
    }
    
    public static CreateEventResponse MapToResponse(this Event createdEvent)
    {
        return new CreateEventResponse
        {
            Id = createdEvent.Id,
            Title = createdEvent.Title,
            Description = createdEvent.Description,
            DateOfEvent = createdEvent.DateOfEvent,
            Reminder = createdEvent.Reminder
        };
    }

    public static EventDetailsResponse MapToResponse(this Event eventDetails, IList<Pet> assignedPets)
    {
        return new EventDetailsResponse
        {
            Id = eventDetails.Id,
            Title = eventDetails.Title,
            Description = eventDetails.Description,
            DateOfEvent = eventDetails.DateOfEvent,
            Reminder = eventDetails.Reminder,
            AssignedPets = assignedPets.Select(MapToAssignedPetDto)
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
}