using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public class GetMedicalNotesQueryHandler : IRequestHandler<GetMedicalNotesQuery, MedicalNotesResponse>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;

    public GetMedicalNotesQueryHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
    }

    public async Task<MedicalNotesResponse> Handle(GetMedicalNotesQuery request, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var medicalNotes = await _medicalNoteRepository.GetAllByPetIdAsync(request.PetId, cancellationToken);

        return medicalNotes.MapToMedicalNotesResponse();
    }
}
