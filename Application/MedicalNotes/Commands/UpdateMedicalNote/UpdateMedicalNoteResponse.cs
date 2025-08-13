using Application.MedicalNotes.Common;

namespace Application.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteResponse : MedicalNoteResponse
{
    public string Description { get; set; }
    public DateTime ModifiedAt { get; set; }
}