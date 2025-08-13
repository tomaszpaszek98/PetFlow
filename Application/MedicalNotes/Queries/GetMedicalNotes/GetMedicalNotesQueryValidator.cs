using FluentValidation;

namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public class GetMedicalNotesQueryValidator : AbstractValidator<GetMedicalNotesQuery>
{
    public GetMedicalNotesQueryValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
    }
}
