using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsResponse>
{
    private readonly INoteRepository _noteRepository;

    public GetNoteDetailsQueryHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<NoteDetailsResponse> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.NoteId, cancellationToken);
        if (note == null)
        {
            throw new NotFoundException(nameof(Note), request.NoteId);
        }
        
        if (note.PetId != request.PetId)
        {
            throw new NotFoundException($"Note with ID {request.NoteId} does not belong to pet with ID {request.PetId}");
        }

        return note.MapToDetailsResponse();
    }
}
