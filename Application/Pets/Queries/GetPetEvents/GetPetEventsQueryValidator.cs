using FluentValidation;

namespace Application.Pets.Queries.GetPetEvents;

public class GetPetEventsQueryValidator : AbstractValidator<GetPetEventsQuery>
{
    public GetPetEventsQueryValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("PetId must be greater than zero.");
    }
}

