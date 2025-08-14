using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Events.Commands.DeletePetFromEvent;

public class DeletePetFromEventCommandHandler : IRequestHandler<DeletePetFromEventCommand>
{
    private readonly IEventRepository _eventRepository;
    private readonly IPetRepository _petRepository;

    public DeletePetFromEventCommandHandler(IEventRepository eventRepository, IPetRepository petRepository)
    {
        _eventRepository = eventRepository;
        _petRepository = petRepository;
    }

    public async Task Handle(DeletePetFromEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity == null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var isDeleted = await _eventRepository.RemovePetFromEventAsync(request.EventId, request.PetId, cancellationToken);
        if (!isDeleted)
        {
            throw new NotFoundException($"Pet with ID {request.PetId} is not assigned to event with ID {request.EventId}");
        }
    }
}
