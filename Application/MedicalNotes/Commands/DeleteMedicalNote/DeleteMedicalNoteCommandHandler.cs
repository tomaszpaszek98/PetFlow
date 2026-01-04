using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommandHandler : IRequestHandler<DeleteMedicalNoteCommand>
{
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    private readonly IPetRepository _petRepository;

    public DeleteMedicalNoteCommandHandler(IMedicalNoteRepository medicalNoteRepository, IPetRepository petRepository)
    {
        _medicalNoteRepository = medicalNoteRepository;
        _petRepository = petRepository;
    }

    public async Task Handle(DeleteMedicalNoteCommand command, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var isDeleted = await _medicalNoteRepository.DeleteAsync(command.MedicalNoteId, command.PetId, cancellationToken);
        if (!isDeleted)
        {
            throw new NotFoundException(nameof(MedicalNote), command.MedicalNoteId);
        }
    }
}
