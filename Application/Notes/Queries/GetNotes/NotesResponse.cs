namespace Application.Notes.Queries.GetNotes;

public class NotesResponse
{
    public IEnumerable<NoteResponseDto> Notes { get; set; }
}