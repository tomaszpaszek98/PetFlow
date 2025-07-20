using Application.Pets.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Pets.Commands.UpdatePet;

public class UpdatePetCommandHandler : IRequestHandler<UpdatePetCommand, PetResponse>
{
    private readonly IPetRepository _repository;

    public UpdatePetCommandHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PetResponse> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
    {
        var pet = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (pet == null)
        {
            throw new EntityNotFoundException($"Pet with id {request.Id} is not exists.");
        }

        UpdatePetProperties(ref pet, request);
        await _repository.UpdateAsync(pet, cancellationToken);
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

