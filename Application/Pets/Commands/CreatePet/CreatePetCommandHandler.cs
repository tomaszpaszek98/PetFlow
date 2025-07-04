using MediatR;

namespace Application.Pets.Commands.CreatePet;

public class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, int>
{
    public Task<int> Handle(CreatePetCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}