using Application.Events.Commands.CreateEvent;
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