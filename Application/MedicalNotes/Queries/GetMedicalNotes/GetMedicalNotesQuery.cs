using MediatR;

namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public class GetMedicalNotesQuery : IRequest<MedicalNotesResponse>
{
    public int PetId { get; set; }
}
