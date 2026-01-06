using Application.MedicalNotes.Commands.CreateMedicalNote;
using Application.MedicalNotes.Commands.UpdateMedicalNote;

namespace PetFlow.Requests.MedicalNote;

public static class RequestMappingExtensions
{
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
}