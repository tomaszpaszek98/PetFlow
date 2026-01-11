using Application.Common.Interfaces.Repositories;
using Application.Notes;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, UpdateNoteResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<UpdateNoteCommandHandler> _logger;

    public UpdateNoteCommandHandler(INoteRepository noteRepository, IPetRepository petRepository, ILogger<UpdateNoteCommandHandler> logger)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<UpdateNoteResponse> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateNoteCommand for PetId: {PetId}, NoteId: {NoteId}", request.PetId, request.NoteId);
        _logger.LogDebug(
            "UpdateNoteCommand details - PetId: {PetId}, NoteId: {NoteId}, Content: {Content}, Type: {Type}",
            request.PetId,
            request.NoteId,
            request.Content,
            request.Type);
        
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var note = await _noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, cancellationToken);
        if (note is null)
        {
            _logger.LogError("Note with ID {NoteId} not found for PetId: {PetId}", request.NoteId, request.PetId);
            throw new NotFoundException(nameof(Note), request.NoteId);
        }

        UpdateNoteProperties(note, request);
        await _noteRepository.UpdateAsync(note, cancellationToken);
        
        _logger.LogInformation("Note with ID {NoteId} updated successfully for PetId: {PetId}", request.NoteId, request.PetId);
        return note.MapToUpdateResponse();
    }

    private static void UpdateNoteProperties(Note note, UpdateNoteCommand request)
    {
        note.Content = request.Content;
        note.Type = request.Type;
    }
}
