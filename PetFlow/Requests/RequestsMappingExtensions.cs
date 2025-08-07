using Application.Events.Commands.UpdateEvent;
using Application.Pets.Commands.UpdatePet;

namespace PetFlow.Requests;

public static class RequestsMappingExtensions
{
    public static UpdatePetCommand MapToCommand(this UpdatePetRequest request, int id)
    {
        return new UpdatePetCommand
        {
            Id = id,
            Name = request.Name,
            Species = request.Species,
            Breed = request.Breed,
            DateOfBirth = request.DateOfBirth
        };
    }

    public static UpdateEventCommand MapToCommand(this UpdateEventRequest request, int id)
    {
        return new UpdateEventCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            DateOfEvent = request.DateOfEvent,
            Reminder = request.Reminder,
            PetToAssignIds = request.PetToAssignIds
        };
    }
}