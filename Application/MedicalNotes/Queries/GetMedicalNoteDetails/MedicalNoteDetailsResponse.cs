using Application.MedicalNotes.Common;

namespace Application.MedicalNotes.Queries.GetMedicalNoteDetails;

public class MedicalNoteDetailsResponse : MedicalNoteResponse
{
    public string Description { get; set; }
    public DateTime ModifiedAt { get; set; }
}