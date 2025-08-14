using Domain.Enums;

namespace PetFlow.Requests.Note;

public class UpdateNoteRequest
{
    public string Content { get; set; }
    // Exceptional use of domain's enum here. 
    public NoteType Type { get; set; }
}