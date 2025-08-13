using Application.MedicalNotes.Common;

namespace Application.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteResponse : MedicalNoteResponse
{
    public string Description { get; set; }
}