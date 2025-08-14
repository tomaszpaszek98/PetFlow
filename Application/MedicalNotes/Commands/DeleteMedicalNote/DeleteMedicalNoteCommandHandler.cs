using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommandHandler : IRequestHandler<DeleteMedicalNoteCommand>
{
    private readonly IMedicalNoteRepository _repository;

    public DeleteMedicalNoteCommandHandler(IMedicalNoteRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteMedicalNoteCommand command, CancellationToken cancellationToken)
    {
        var medicalNote = await _repository.GetByIdAsync(command.MedicalNoteId, cancellationToken);
        if (medicalNote == null)
        {
            throw new NotFoundException(nameof(MedicalNote), command.MedicalNoteId);
        }
        
        if (medicalNote.PetId != command.PetId)
        {
            throw new NotFoundException($"Medical note with ID {command.MedicalNoteId} does not belong to pet with ID {command.PetId}");
        }
        
        await _repository.DeleteByIdAsync(command.MedicalNoteId, cancellationToken);
    }
}
