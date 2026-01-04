using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;

    public DeleteNoteCommandHandler(INoteRepository noteRepository, IPetRepository petRepository)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
    }

    public async Task Handle(DeleteNoteCommand command, CancellationToken cancellationToken)
    {
        var petExists = await _petRepository.ExistsAsync(command.PetId, cancellationToken);
        if (!petExists)
        {
            throw new NotFoundException(nameof(Pet), command.PetId);
        }

        var isDeleted = await _noteRepository.DeleteAsync(command.NoteId, command.PetId, cancellationToken);
        if (!isDeleted)
        {
            throw new NotFoundException(nameof(Note), command.NoteId);
        }
    }
}
