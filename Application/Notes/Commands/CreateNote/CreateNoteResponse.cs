using Application.Notes.Common;

namespace Application.Notes.Commands.CreateNote;

public class CreateNoteResponse : NoteResponse
{
    public string Content { get; set; }
}