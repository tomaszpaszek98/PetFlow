using MediatR;

namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public record GetMedicalNotesQuery : IRequest<MedicalNotesResponse>
{
    public int PetId { get; init; }
}
