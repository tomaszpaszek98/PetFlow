using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommandHandler : IRequestHandler<DeleteMedicalNoteCommand>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<DeleteMedicalNoteCommandHandler> _logger;

    public DeleteMedicalNoteCommandHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository, ILogger<DeleteMedicalNoteCommandHandler> logger)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteMedicalNoteCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteMedicalNoteCommand for PetId: {PetId}, MedicalNoteId: {MedicalNoteId}", command.PetId, command.MedicalNoteId);
        
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", command.PetId);
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var isDeleted = await _medicalNoteRepository.DeleteAsync(command.MedicalNoteId, command.PetId, cancellationToken);
        if (!isDeleted)
        {
            _logger.LogError("MedicalNote with ID {MedicalNoteId} not found for PetId: {PetId}", command.MedicalNoteId, command.PetId);
            throw new NotFoundException(nameof(MedicalNote), command.MedicalNoteId);
        }
        
        _logger.LogInformation("MedicalNote with ID {MedicalNoteId} deleted successfully for PetId: {PetId}", command.MedicalNoteId, command.PetId);
    }
}
