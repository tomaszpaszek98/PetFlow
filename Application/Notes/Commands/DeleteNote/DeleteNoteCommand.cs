using MediatR;

namespace Application.Notes.Commands.DeleteNote;

public class DeleteNoteCommand : IRequest
{
    public int PetId { get; set; }
    public int NoteId { get; set; }
}