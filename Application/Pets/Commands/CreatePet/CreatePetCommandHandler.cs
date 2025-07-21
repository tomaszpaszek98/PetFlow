using System.Runtime.CompilerServices;
using Application.Pets.Common;
using Application.Pets.Queries.GetPetDetails;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Pets.Commands.CreatePet;

public class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, PetResponse>
{
    private readonly IPetRepository _repository;

    public CreatePetCommandHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PetResponse> Handle(CreatePetCommand request, CancellationToken cancellationToken = default)
    {
        var pet = request.MapToPet();
        var petHasBeenCreated = await _repository.CreateAsync(pet, cancellationToken);

        if (petHasBeenCreated is false)
        {
            throw new EntityCreationException($"Unexpected error when creating Pet with name {request.Name}");
        }

        return pet.MapToResponse();
    }
}