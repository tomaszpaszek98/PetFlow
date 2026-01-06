using Domain.Enums;
using MediatR;

namespace Application.Notes.Commands.UpdateNote;

public record UpdateNoteCommand : IRequest<UpdateNoteResponse>
{
    public int PetId { get; init; }
    public int NoteId { get; init; }
    public string Content { get; init; }
    public NoteType Type { get; init; }
}