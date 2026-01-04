using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

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
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var medicalNote = await _medicalNoteRepository.GetByIdWithPetAsync(request.MedicalNoteId, request.PetId, cancellationToken);
        if (medicalNote == null)
        {
            throw new NotFoundException(nameof(MedicalNote), request.MedicalNoteId);
        }

        return medicalNote.MapToDetailsResponse();
    }
}
