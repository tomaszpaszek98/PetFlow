using FluentValidation;

namespace Application.Notes.Queries.GetNotes;

public class GetNotesQueryValidator : AbstractValidator<GetNotesQuery>
{
    public GetNotesQueryValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
    }
}
