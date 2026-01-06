using MediatR;

namespace Application.Notes.Queries.GetNoteDetails;

public record GetNoteDetailsQuery : IRequest<NoteDetailsResponse>
{
    public int PetId { get; init; }
    public int NoteId { get; init; }
}
