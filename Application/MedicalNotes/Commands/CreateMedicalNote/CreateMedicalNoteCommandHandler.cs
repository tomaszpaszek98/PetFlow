using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteCommandHandler : IRequestHandler<CreateMedicalNoteCommand, CreateMedicalNoteResponse>
{
    private readonly IMedicalNoteRepository _repository;
    private readonly IPetRepository _petRepository;

    public CreateMedicalNoteCommandHandler(IMedicalNoteRepository repository, IPetRepository petRepository)
    {
        _repository = repository;
        _petRepository = petRepository;
    }

    public async Task<CreateMedicalNoteResponse> Handle(CreateMedicalNoteCommand command, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var requestedNote = command.MapToMedicalNote();
        var createdNote = await _repository.CreateAsync(requestedNote, cancellationToken);
        
        return createdNote.MapToCreateResponse();
    }
}
