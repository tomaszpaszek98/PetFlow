using Application.Events.Commands.AddPetToEvent;
using Application.Events.Commands.UpdateEvent;
using Application.Pets.Commands.CreatePet;
using PetFlow.Requests.Pet;

namespace PetFlow.Requests.Event;

public static class RequestMappingExtensions
{
    public static UpdateEventCommand MapToCommand(this UpdateEventRequest request, int id)
    {
        return new UpdateEventCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            DateOfEvent = request.DateOfEvent,
            Reminder = request.Reminder,
            AssignedPetsIds = request.PetToAssignIds
        };
    }
    
    public static AddPetToEventCommand MapToCommand(this AddPetToEventRequest request, int eventId)
    {
        return new AddPetToEventCommand
        {
            PetId = request.PetId,
            EventId = eventId
        };
    }
}