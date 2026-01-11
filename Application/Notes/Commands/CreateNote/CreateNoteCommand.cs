using Domain.Enums;
using MediatR;

namespace Application.Notes.Commands.CreateNote;

public record CreateNoteCommand : IRequest<CreateNoteResponse>
{
    public int PetId { get; init; }
    public string Content { get; init; } 
    public NoteType Type { get; init; }
}