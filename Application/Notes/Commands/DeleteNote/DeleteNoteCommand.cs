using MediatR;

namespace Application.Notes.Commands.DeleteNote;

public record DeleteNoteCommand : IRequest
{
    public int PetId { get; init; }
    public int NoteId { get; init; }
}