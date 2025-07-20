using MediatR;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.Pets.Commands.DeletePet;

public class DeletePetCommandHandler : IRequestHandler<DeletePetCommand, bool>
{
    private readonly IPetRepository _repository;

    public DeletePetCommandHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeletePetCommand request, CancellationToken cancellationToken = default)
    {
        var isDeleted = await _repository.DeleteByIdAsync(request.PetId, cancellationToken);
        
        if (isDeleted is false)
        {
            throw new EntityNotFoundException($"Pet with id {request.PetId} is not exists.");
        }
        return true;
    }
}

