using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

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
        var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet is null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var medicalNote = await _medicalNoteRepository.GetByIdAsync(request.MedicalNoteId, cancellationToken);
        if (medicalNote is null)
        {
            throw new NotFoundException(nameof(MedicalNote), request.MedicalNoteId);
        }
        
        if (medicalNote.PetId != request.PetId)
        {
            throw new NotFoundException($"Medical note with ID {request.MedicalNoteId} does not belong to pet with ID {request.PetId}");
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
