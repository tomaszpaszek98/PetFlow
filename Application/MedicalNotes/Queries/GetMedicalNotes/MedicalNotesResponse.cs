namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public class MedicalNotesResponse
{
    public IEnumerable<MedicalNoteResponseDto> MedicalNotes { get; set; }
}