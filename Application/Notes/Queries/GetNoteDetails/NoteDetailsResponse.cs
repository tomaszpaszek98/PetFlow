using Application.Notes.Common;

namespace Application.Notes.Queries.GetNoteDetails;

public class NoteDetailsResponse : NoteResponse
{
    public string Content { get; set; }
    public DateTime ModifiedAt { get; set; }
}
