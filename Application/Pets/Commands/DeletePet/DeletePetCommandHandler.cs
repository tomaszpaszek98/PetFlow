using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Pets.Commands.DeletePet;

public class DeletePetCommandHandler : IRequestHandler<DeletePetCommand>
{
    private readonly IPetRepository _repository;
    private readonly ILogger<DeletePetCommandHandler> _logger;

    public DeletePetCommandHandler(IPetRepository repository, ILogger<DeletePetCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(DeletePetCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling DeletePetCommand for PetId: {PetId}", request.PetId);
        var isDeleted = await _repository.DeleteAsync(request.PetId, cancellationToken);
        
        if (isDeleted is false)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        _logger.LogInformation("Pet with ID {PetId} deleted successfully", request.PetId);
    }
}
