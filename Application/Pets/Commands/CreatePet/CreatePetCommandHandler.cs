using Application.Pets.Common;
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
        var createdPet = await _repository.CreateAsync(pet, cancellationToken);
        
        return createdPet.MapToResponse();
    }
}