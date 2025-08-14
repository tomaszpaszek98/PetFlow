using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
{
    private readonly INoteRepository _repository;

    public DeleteNoteCommandHandler(INoteRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteNoteCommand command, CancellationToken cancellationToken)
    {
        var note = await _repository.GetByIdAsync(command.NoteId, cancellationToken);
        if (note == null)
        {
            throw new NotFoundException(nameof(Note), command.NoteId);
        }
        
        if (note.PetId != command.PetId)
        {
            throw new NotFoundException($"Note with ID {command.NoteId} does not belong to pet with ID {command.PetId}");
        }
        
        await _repository.DeleteByIdAsync(command.NoteId, cancellationToken);
    }
}
