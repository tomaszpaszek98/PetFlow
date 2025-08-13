using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Notes.Commands.CreateNote;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteResponse>
{
    private readonly INoteRepository _repository;
    private readonly IPetRepository _petRepository;

    public CreateNoteCommandHandler(INoteRepository repository, IPetRepository petRepository)
    {
        _repository = repository;
        _petRepository = petRepository;
    }

    public async Task<CreateNoteResponse> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdAsync(command.PetId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), command.PetId);
        }
        
        var requestedNote = command.MapToNote();
        var createdNote = await _repository.CreateAsync(requestedNote, cancellationToken);

        return createdNote.MapToCreateResponse();
    }
}
