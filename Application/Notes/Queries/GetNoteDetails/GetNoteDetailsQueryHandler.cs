using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetNoteDetailsQueryHandler> _logger;

    public GetNoteDetailsQueryHandler(INoteRepository noteRepository, IPetRepository petRepository, ILogger<GetNoteDetailsQueryHandler> logger)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<NoteDetailsResponse> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetNoteDetailsQuery for PetId: {PetId}, NoteId: {NoteId}", request.PetId, request.NoteId);
        
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }

        var note = await _noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, cancellationToken);
        if (note == null)
        {
            _logger.LogError("Note with ID {NoteId} not found for PetId: {PetId}", request.NoteId, request.PetId);
            throw new NotFoundException(nameof(Note), request.NoteId);
        }

        _logger.LogInformation("Note details retrieved successfully for NoteId: {NoteId}, PetId: {PetId}", request.NoteId, request.PetId);
        return note.MapToDetailsResponse();
    }
}
