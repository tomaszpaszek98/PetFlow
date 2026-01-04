using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Events.Commands.DeletePetFromEvent;

public class DeletePetFromEventCommandHandler : IRequestHandler<DeletePetFromEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public DeletePetFromEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(DeletePetFromEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdWithPetEventsTrackedAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        var petEventToRemove = eventEntity.PetEvents.FirstOrDefault(pe => pe.PetId == request.PetId);
        if (petEventToRemove is null)
        {
            throw new NotFoundException($"Pet with ID {request.PetId} is not assigned to event with ID {request.EventId}");
        }
        
        eventEntity.PetEvents.Remove(petEventToRemove);
        await _eventRepository.UpdateAsync(eventEntity, cancellationToken);
    }
}