using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.MedicalNotes.Queries.GetMedicalNoteDetails;

public class GetMedicalNoteDetailsQueryHandler : IRequestHandler<GetMedicalNoteDetailsQuery, MedicalNoteDetailsResponse>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;

    public GetMedicalNoteDetailsQueryHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
    }

    public async Task<MedicalNoteDetailsResponse> Handle(GetMedicalNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        await ValidateIfPetExistsAsync(request.PetId, cancellationToken);
        
        var medicalNote = await _medicalNoteRepository.GetByIdAsync(request.MedicalNoteId, cancellationToken);
        if (medicalNote == null)
        {
            throw new NotFoundException(nameof(MedicalNote), request.MedicalNoteId);
        }
        
        if (medicalNote.PetId != request.PetId)
        {
            throw new NotFoundException($"Medical note with ID {request.MedicalNoteId} does not belong to pet with ID {request.PetId}");
        }

        return medicalNote.MapToDetailsResponse();
    }
    
    private async Task ValidateIfPetExistsAsync(int petId, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdAsync(petId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), petId);
        }
    }
}
