using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteCommandHandler : IRequestHandler<UpdateMedicalNoteCommand, UpdateMedicalNoteResponse>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<UpdateMedicalNoteCommandHandler> _logger;

    public UpdateMedicalNoteCommandHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository, ILogger<UpdateMedicalNoteCommandHandler> logger)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<UpdateMedicalNoteResponse> Handle(UpdateMedicalNoteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateMedicalNoteCommand for PetId: {PetId}, MedicalNoteId: {MedicalNoteId}", request.PetId, request.MedicalNoteId);
        _logger.LogDebug(
            "UpdateMedicalNoteCommand details - PetId: {PetId}, MedicalNoteId: {MedicalNoteId}, Title: {Title}, Description: {Description}",
            request.PetId,
            request.MedicalNoteId,
            request.Title,
            request.Description);
        
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var medicalNote = await _medicalNoteRepository.GetByIdWithPetAsync(request.MedicalNoteId, request.PetId, cancellationToken);
        if (medicalNote is null)
        {
            _logger.LogError("MedicalNote with ID {MedicalNoteId} not found for PetId: {PetId}", request.MedicalNoteId, request.PetId);
            throw new NotFoundException(nameof(MedicalNote), request.MedicalNoteId);
        }

        UpdateMedicalNoteProperties(medicalNote, request);
        await _medicalNoteRepository.UpdateAsync(medicalNote, cancellationToken);
        
        _logger.LogInformation("MedicalNote with ID {MedicalNoteId} updated successfully for PetId: {PetId}", request.MedicalNoteId, request.PetId);
        return medicalNote.MapToUpdateResponse();
    }
    
    private static void UpdateMedicalNoteProperties(MedicalNote medicalNote, UpdateMedicalNoteCommand request)
    {
        medicalNote.Title = request.Title;
        medicalNote.Description = request.Description;
    }
}
