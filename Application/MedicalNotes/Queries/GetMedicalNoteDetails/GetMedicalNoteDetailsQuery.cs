using MediatR;

namespace Application.MedicalNotes.Queries.GetMedicalNoteDetails;

public record GetMedicalNoteDetailsQuery : IRequest<MedicalNoteDetailsResponse>
{
    public int PetId { get; init; }
    public int MedicalNoteId { get; init; }
}