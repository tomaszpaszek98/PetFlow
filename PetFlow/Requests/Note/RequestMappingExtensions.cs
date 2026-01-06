using Application.Notes.Commands.CreateNote;
using Application.Notes.Commands.UpdateNote;

namespace PetFlow.Requests.Note;

public static class RequestMappingExtensions
{
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
}