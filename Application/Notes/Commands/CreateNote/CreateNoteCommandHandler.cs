using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Notes.Commands.CreateNote;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;

    public CreateNoteCommandHandler(INoteRepository noteRepository, IPetRepository petRepository)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
    }

    public async Task<CreateNoteResponse> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var requestedNote = command.MapToNote();
        var createdNote = await _noteRepository.CreateAsync(requestedNote, cancellationToken);
        
        return createdNote.MapToCreateResponse();
    }
}
