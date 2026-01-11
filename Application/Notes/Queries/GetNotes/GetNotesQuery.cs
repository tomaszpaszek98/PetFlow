using MediatR;

namespace Application.Notes.Queries.GetNotes;

public record GetNotesQuery : IRequest<NotesResponse>
{
    public int PetId { get; init; }
}