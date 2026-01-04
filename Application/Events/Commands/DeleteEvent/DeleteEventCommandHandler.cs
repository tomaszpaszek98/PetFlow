using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Domain.Exceptions;

namespace Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IEventRepository _repository;

    public DeleteEventCommandHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken = default)
    {
        var isDeleted = await _repository.DeleteAsync(request.EventId, cancellationToken);
        
        if (isDeleted is false)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }
    }
}