using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

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
        await ValidateIfPetExistsAsync(request.PetId, cancellationToken);
        
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
    
    private async Task ValidateIfPetExistsAsync(int petId, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdAsync(petId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), petId);
        }
    }
}
