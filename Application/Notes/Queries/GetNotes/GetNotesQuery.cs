using MediatR;

namespace Application.Notes.Queries.GetNotes;

public class GetNotesQuery : IRequest<NotesResponse>
{
    public int PetId { get; set; }
}