using Application.Common.Interfaces.Repositories;
using Application.Notes;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

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
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var note = await _noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, cancellationToken);
        if (note is null)
        {
            throw new NotFoundException(nameof(Note), request.NoteId);
        }

        UpdateNoteProperties(note, request);
        await _noteRepository.UpdateAsync(note, cancellationToken);
        
        return note.MapToUpdateResponse();
    }

    private static void UpdateNoteProperties(Note note, UpdateNoteCommand request)
    {
        note.Content = request.Content;
        note.Type = request.Type;
    }
}
