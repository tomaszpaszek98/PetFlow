using Application.Common.Interfaces.Repositories;
using Application.Pets.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Pets.Commands.UpdatePet;

public class UpdatePetCommandHandler : IRequestHandler<UpdatePetCommand, PetResponse>
{
    private readonly IPetRepository _repository;
    private readonly ILogger<UpdatePetCommandHandler> _logger;

    public UpdatePetCommandHandler(IPetRepository repository, ILogger<UpdatePetCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PetResponse> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdatePetCommand for PetId: {PetId}", request.Id);
        _logger.LogDebug(
            "UpdatePetCommand details - PetId: {PetId}, Name: {Name}, Species: {Species}, Breed: {Breed}, DateOfBirth: {DateOfBirth}",
            request.Id,
            request.Name,
            request.Species,
            request.Breed,
            request.DateOfBirth);
        
        var pet = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (pet == null)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.Id);
            throw new NotFoundException(nameof(Pet), request.Id);
        }

        UpdatePetProperties(ref pet, request);
        await _repository.UpdateAsync(pet, cancellationToken);
        
        _logger.LogInformation("Pet with ID {PetId} updated successfully", request.Id);
        return pet.MapToResponse();
    }

    private static void UpdatePetProperties(ref Pet pet, UpdatePetCommand request)
    {
        pet.Name = request.Name;
        pet.Species = request.Species;
        pet.Breed = request.Breed;
        pet.DateOfBirth = request.DateOfBirth;
    }
}