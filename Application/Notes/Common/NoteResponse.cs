using Domain.Enums;

namespace Application.Notes.Common;

public class NoteResponse
{
    public int Id { get; set; }
    public NoteType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}