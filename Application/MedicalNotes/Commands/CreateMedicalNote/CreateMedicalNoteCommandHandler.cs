using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteCommandHandler : IRequestHandler<CreateMedicalNoteCommand, CreateMedicalNoteResponse>
{
    private readonly IMedicalNoteRepository _repository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<CreateMedicalNoteCommandHandler> _logger;

    public CreateMedicalNoteCommandHandler(IMedicalNoteRepository repository, IPetRepository petRepository, ILogger<CreateMedicalNoteCommandHandler> logger)
    {
        _repository = repository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<CreateMedicalNoteResponse> Handle(CreateMedicalNoteCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateMedicalNoteCommand for PetId: {PetId}", command.PetId);
        _logger.LogDebug(
            "CreateMedicalNoteCommand details - PetId: {PetId}, Title: {Title}, Description: {Description}",
            command.PetId,
            command.Title,
            command.Description);
        
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", command.PetId);
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var requestedNote = command.MapToMedicalNote();
        var createdNote = await _repository.CreateAsync(requestedNote, cancellationToken);
        
        _logger.LogInformation("MedicalNote created successfully with ID {MedicalNoteId} for PetId: {PetId}", createdNote.Id, command.PetId);
        return createdNote.MapToCreateResponse();
    }
}
