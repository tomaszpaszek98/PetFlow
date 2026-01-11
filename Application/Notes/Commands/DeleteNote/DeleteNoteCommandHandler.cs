using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<DeleteNoteCommandHandler> _logger;

    public DeleteNoteCommandHandler(INoteRepository noteRepository, IPetRepository petRepository, ILogger<DeleteNoteCommandHandler> logger)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteNoteCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteNoteCommand for PetId: {PetId}, NoteId: {NoteId}", command.PetId, command.NoteId);
        
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", command.PetId);
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var isDeleted = await _noteRepository.DeleteAsync(command.NoteId, command.PetId, cancellationToken);
        if (!isDeleted)
        {
            _logger.LogError("Note with ID {NoteId} not found for PetId: {PetId}", command.NoteId, command.PetId);
            throw new NotFoundException(nameof(Note), command.NoteId);
        }
        
        _logger.LogInformation("Note with ID {NoteId} deleted successfully for PetId: {PetId}", command.NoteId, command.PetId);
    }
}
