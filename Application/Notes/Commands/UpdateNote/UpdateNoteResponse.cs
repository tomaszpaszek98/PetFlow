using Application.Notes.Common;

namespace Application.Notes.Commands.UpdateNote;

public class UpdateNoteResponse : NoteResponse
{
    public string Content { get; set; }
    public DateTime ModifiedAt { get; set; }
}