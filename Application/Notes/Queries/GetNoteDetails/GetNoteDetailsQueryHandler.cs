using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;

    public GetNoteDetailsQueryHandler(INoteRepository noteRepository, IPetRepository petRepository)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
    }

    public async Task<NoteDetailsResponse> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var note = await _noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, cancellationToken);
        if (note == null)
        {
            throw new NotFoundException(nameof(Note), request.NoteId);
        }

        return note.MapToDetailsResponse();
    }
}
