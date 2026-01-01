using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteCommandHandler : IRequestHandler<UpdateMedicalNoteCommand, UpdateMedicalNoteResponse>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;

    public UpdateMedicalNoteCommandHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
    }

    public async Task<UpdateMedicalNoteResponse> Handle(UpdateMedicalNoteCommand request, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var medicalNote = await _medicalNoteRepository.GetByIdWithPetAsync(request.MedicalNoteId, request.PetId, cancellationToken);
        if (medicalNote is null)
        {
            throw new NotFoundException(nameof(MedicalNote), request.MedicalNoteId);
        }

        UpdateMedicalNoteProperties(medicalNote, request);
        await _medicalNoteRepository.UpdateAsync(medicalNote, cancellationToken);
        
        return medicalNote.MapToUpdateResponse();
    }
    
    private static void UpdateMedicalNoteProperties(MedicalNote medicalNote, UpdateMedicalNoteCommand request)
    {
        medicalNote.Title = request.Title;
        medicalNote.Description = request.Description;
    }
}
