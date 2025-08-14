using Domain.Enums;
using MediatR;

namespace Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommand : IRequest<UpdateNoteResponse>
{
    public int PetId { get; set; }
    public int NoteId { get; set; }
    public string Content { get; set; }
    public NoteType Type { get; set; }
}