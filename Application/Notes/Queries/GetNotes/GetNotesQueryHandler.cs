using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notes.Queries.GetNotes;

public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, NotesResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetNotesQueryHandler> _logger;

    public GetNotesQueryHandler(INoteRepository noteRepository, IPetRepository petRepository, ILogger<GetNotesQueryHandler> logger)
    {
        _noteRepository = noteRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<NotesResponse> Handle(GetNotesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetNotesQuery for PetId: {PetId}", request.PetId);
        
        var petExists = await _petRepository.ExistsAsync(request.PetId, cancellationToken);
        if (!petExists)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var notes = (await _noteRepository.GetAllByPetIdAsync(request.PetId, cancellationToken)).ToList();

        _logger.LogInformation("Retrieved {Count} notes for PetId: {PetId}", notes.Count, request.PetId);
        return notes.MapToNotesResponse();
    }
}
