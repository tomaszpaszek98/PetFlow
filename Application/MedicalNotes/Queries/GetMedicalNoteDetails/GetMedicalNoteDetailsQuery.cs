using MediatR;

namespace Application.MedicalNotes.Queries.GetMedicalNoteDetails;

public class GetMedicalNoteDetailsQuery : IRequest<MedicalNoteDetailsResponse>
{
    public int PetId { get; set; }
    public int MedicalNoteId { get; set; }
}