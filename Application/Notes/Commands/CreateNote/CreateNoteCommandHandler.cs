using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notes.Commands.CreateNote;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<CreateNoteCommandHandler> _logger;

    public CreateNoteCommandHandler(INoteRepository noteRepository, IPetRepository petRepository, ILogger<CreateNoteCommandHandler> logger)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<CreateNoteResponse> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateNoteCommand for PetId: {PetId}", command.PetId);
        _logger.LogDebug(
            "CreateNoteCommand details - PetId: {PetId}, Content: {Content}, Type: {Type}",
            command.PetId,
            command.Content,
            command.Type);
        
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", command.PetId);
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var requestedNote = command.MapToNote();
        var createdNote = await _noteRepository.CreateAsync(requestedNote, cancellationToken);
        
        _logger.LogInformation("Note created successfully with ID {NoteId} for PetId: {PetId}", createdNote.Id, command.PetId);
        return createdNote.MapToCreateResponse();
    }
}
