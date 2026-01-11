using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IEventRepository _repository;
    private readonly ILogger<DeleteEventCommandHandler> _logger;

    public DeleteEventCommandHandler(IEventRepository repository, ILogger<DeleteEventCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling DeleteEventCommand for EventId: {EventId}", request.EventId);
        var isDeleted = await _repository.DeleteAsync(request.EventId, cancellationToken);
        
        if (isDeleted is false)
        {
            _logger.LogError("Event with ID {EventId} not found", request.EventId);
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        _logger.LogInformation("Event with ID {EventId} deleted successfully", request.EventId);
    }
}