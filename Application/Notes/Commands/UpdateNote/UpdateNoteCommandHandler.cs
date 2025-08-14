using System.Runtime.CompilerServices;
using Application.Common.Interfaces.Repositories;
using Application.Notes;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, UpdateNoteResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;

    public UpdateNoteCommandHandler(INoteRepository noteRepository, IPetRepository petRepository)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
    }

    public async Task<UpdateNoteResponse> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        await ValidateIfPetExistsAsync(request.PetId, cancellationToken);
        
        var note = await _noteRepository.GetByIdAsync(request.NoteId, cancellationToken);
        if (note is null)
        {
            throw new NotFoundException(nameof(Note), request.NoteId);
        }
        
        if (note.PetId != request.PetId)
        {
            throw new NotFoundException($"Note with ID {request.NoteId} does not belong to pet with ID {request.PetId}");
        }

        UpdateNoteProperties(note, request);
        await _noteRepository.UpdateAsync(note, cancellationToken);
        
        return note.MapToUpdateResponse();
    }
    
    private async Task ValidateIfPetExistsAsync(int petId, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdAsync(petId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), petId);
        }
    }
    
    private static void UpdateNoteProperties(Note note, UpdateNoteCommand request)
    {
        note.Content = request.Content;
        note.Type = request.Type;
    }
}
