using Domain.Enums;
using MediatR;

namespace Application.Notes.Commands.CreateNote;

public class CreateNoteCommand : IRequest<CreateNoteResponse>
{
    public int PetId { get; set; }
    public string Content { get; set; } 
    public NoteType Type { get; set; }
}