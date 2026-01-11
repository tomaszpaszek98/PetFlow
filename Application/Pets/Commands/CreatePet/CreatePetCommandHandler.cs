using Application.Common.Interfaces.Repositories;
using Application.Pets.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Pets.Commands.CreatePet;

public class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, PetResponse>
{
    private readonly IPetRepository _repository;
    private readonly ILogger<CreatePetCommandHandler> _logger;

    public CreatePetCommandHandler(IPetRepository repository, ILogger<CreatePetCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PetResponse> Handle(CreatePetCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling CreatePetCommand with Name: {Name}", request.Name);
        _logger.LogDebug(
            "CreatePetCommand details - Name: {Name}, Species: {Species}, Breed: {Breed}, DateOfBirth: {DateOfBirth}",
            request.Name,
            request.Species,
            request.Breed,
            request.DateOfBirth);
        
        var pet = request.MapToPet();
        var createdPet = await _repository.CreateAsync(pet, cancellationToken);
        
        _logger.LogInformation("Pet created successfully with ID {PetId}", createdPet.Id);
        return createdPet.MapToResponse();
    }
}