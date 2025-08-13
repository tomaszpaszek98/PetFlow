using MediatR;

namespace Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQuery : IRequest<NoteDetailsResponse>
{
    public int PetId { get; set; }
    public int NoteId { get; set; }
}
