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
        var eventEntity = await _eventRepository.GetByIdWithPetsAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        var petToRemove = eventEntity.Pets.FirstOrDefault(p => p.Id == request.PetId);
        if (petToRemove is null)
        {
            throw new NotFoundException($"Pet with ID {request.PetId} is not assigned to event with ID {request.EventId}");
        }
        
        eventEntity.Pets.Remove(petToRemove);
        await _eventRepository.UpdateAsync(eventEntity, cancellationToken);
    }
}
