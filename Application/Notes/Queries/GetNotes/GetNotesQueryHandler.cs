using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Notes.Queries.GetNotes;

public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, NotesResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;

    public GetNotesQueryHandler(INoteRepository noteRepository, IPetRepository petRepository)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
    }

    public async Task<NotesResponse> Handle(GetNotesQuery request, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var notes = await _noteRepository.GetAllByPetIdAsync(request.PetId, cancellationToken);

        return notes.MapToNotesResponse();
    }
}
