using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

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
        var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var medicalNotes = await _medicalNoteRepository.GetAllByPetIdAsync(request.PetId, cancellationToken);

        return medicalNotes.MapToMedicalNotesResponse();
    }
}
