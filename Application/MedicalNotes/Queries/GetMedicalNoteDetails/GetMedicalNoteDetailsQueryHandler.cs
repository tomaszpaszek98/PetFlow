using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.MedicalNotes.Queries.GetMedicalNoteDetails;

public class GetMedicalNoteDetailsQueryHandler : IRequestHandler<GetMedicalNoteDetailsQuery, MedicalNoteDetailsResponse>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetMedicalNoteDetailsQueryHandler> _logger;

    public GetMedicalNoteDetailsQueryHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository, ILogger<GetMedicalNoteDetailsQueryHandler> logger)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<MedicalNoteDetailsResponse> Handle(GetMedicalNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetMedicalNoteDetailsQuery for PetId: {PetId}, MedicalNoteId: {MedicalNoteId}", request.PetId, request.MedicalNoteId);
        
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var medicalNote = await _medicalNoteRepository.GetByIdWithPetAsync(request.MedicalNoteId, request.PetId, cancellationToken);
        if (medicalNote == null)
        {
            _logger.LogError("MedicalNote with ID {MedicalNoteId} not found for PetId: {PetId}", request.MedicalNoteId, request.PetId);
            throw new NotFoundException(nameof(MedicalNote), request.MedicalNoteId);
        }

        _logger.LogInformation("MedicalNote details retrieved successfully for MedicalNoteId: {MedicalNoteId}, PetId: {PetId}", request.MedicalNoteId, request.PetId);
        return medicalNote.MapToDetailsResponse();
    }
}
