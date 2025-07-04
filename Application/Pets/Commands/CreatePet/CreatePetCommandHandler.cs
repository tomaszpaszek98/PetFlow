using MediatR;

namespace Application.Pets.Commands.CreatePet;

public class CreatePetComandHandler : IRequestHandler<CreatePetCommand, int>
{
    public Task<int> Handle(CreatePetCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}