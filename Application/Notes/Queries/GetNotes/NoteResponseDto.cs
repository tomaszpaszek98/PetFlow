using Application.Notes.Common;

namespace Application.Notes.Queries.GetNotes;

public class NoteResponseDto : NoteResponse
{
    public string ShortContent { get; set; }
    public DateTime ModifiedAt { get; set; }
}