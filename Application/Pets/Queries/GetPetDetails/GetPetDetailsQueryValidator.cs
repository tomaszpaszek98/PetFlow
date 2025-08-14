using FluentValidation;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryValidator : AbstractValidator<GetPetDetailsQuery>
{
    public GetPetDetailsQueryValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
    }
}
