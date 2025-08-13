using Domain.Enums;

namespace PetFlow.Requests;

public class CreateNoteRequest
{
    public string Content { get; set; }
    // Exceptional use of domain's enum here. 
    public NoteType Type { get; set; }
}