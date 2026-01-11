using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Commands.DeletePetFromEvent;

public class DeletePetFromEventCommandHandler : IRequestHandler<DeletePetFromEventCommand>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<DeletePetFromEventCommandHandler> _logger;

    public DeletePetFromEventCommandHandler(IEventRepository eventRepository, ILogger<DeletePetFromEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task Handle(DeletePetFromEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeletePetFromEventCommand for EventId: {EventId}, PetId: {PetId}", request.EventId, request.PetId);
        
        var eventEntity = await _eventRepository.GetByIdWithPetEventsTrackedAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
        {
            _logger.LogError("Event with ID {EventId} not found", request.EventId);
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        var petEventToRemove = eventEntity.PetEvents.FirstOrDefault(pe => pe.PetId == request.PetId);
        if (petEventToRemove is null)
        {
            _logger.LogError("Pet with ID {PetId} is not assigned to event with ID {EventId}", request.PetId, request.EventId);
            throw new NotFoundException($"Pet with ID {request.PetId} is not assigned to event with ID {request.EventId}");
        }
        
        eventEntity.PetEvents.Remove(petEventToRemove);
        await _eventRepository.UpdateAsync(eventEntity, cancellationToken);
        
        _logger.LogInformation("Pet with ID {PetId} removed successfully from Event with ID {EventId}", request.PetId, request.EventId);
    }
}