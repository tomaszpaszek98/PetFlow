using Application.Events.Commands.AddPetToEvent;
using Application.Events.Commands.UpdateEvent;
using Application.MedicalNotes.Commands.CreateMedicalNote;
using Application.MedicalNotes.Commands.UpdateMedicalNote;
using Application.Notes.Commands.CreateNote;
using Application.Notes.Commands.UpdateNote;
using Application.Pets.Commands.UpdatePet;
using PetFlow.Requests.Event;
using PetFlow.Requests.MedicalNote;
using PetFlow.Requests.Note;
using PetFlow.Requests.Pet;

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
            AssignedPetsIds = request.PetToAssignIds
        };
    }

    public static CreateMedicalNoteCommand MapToCommand(this CreateMedicalNoteRequest request, int petId)
    {
        return new CreateMedicalNoteCommand
        {
            PetId = petId,
            Title = request.Title,
            Description = request.Description
        };
    }
    
    public static UpdateMedicalNoteCommand MapToCommand(this UpdateMedicalNoteRequest request, int petId, int medicalNoteId)
    {
        return new UpdateMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = medicalNoteId,
            Title = request.Title,
            Description = request.Description
        };
    }

    public static CreateNoteCommand MapToCommand(this CreateNoteRequest request, int petId)
    {
        return new CreateNoteCommand
        {
            PetId = petId,
            Content = request.Content,
            Type = request.Type
        };
    }

    public static UpdateNoteCommand MapToCommand(this UpdateNoteRequest request, int petId, int medicalNoteId)
    {
        return new UpdateNoteCommand
        {
            PetId = petId,
            NoteId = medicalNoteId,
            Content = request.Content,
            Type = request.Type
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