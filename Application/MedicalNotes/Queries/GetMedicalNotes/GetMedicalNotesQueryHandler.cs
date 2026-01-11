using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public class GetMedicalNotesQueryHandler : IRequestHandler<GetMedicalNotesQuery, MedicalNotesResponse>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetMedicalNotesQueryHandler> _logger;

    public GetMedicalNotesQueryHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository, ILogger<GetMedicalNotesQueryHandler> logger)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<MedicalNotesResponse> Handle(GetMedicalNotesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetMedicalNotesQuery for PetId: {PetId}", request.PetId);
        
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var medicalNotes = (await _medicalNoteRepository.GetAllByPetIdAsync(request.PetId, cancellationToken)).ToList();

        _logger.LogInformation("Retrieved {Count} medical notes for PetId: {PetId}", medicalNotes.Count, request.PetId);
        return medicalNotes.MapToMedicalNotesResponse();
    }
}
