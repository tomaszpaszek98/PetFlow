using FluentValidation;

namespace Application.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommandValidator : AbstractValidator<DeleteMedicalNoteCommand>
{
    public DeleteMedicalNoteCommandValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
            
        RuleFor(x => x.MedicalNoteId)
            .GreaterThan(0)
            .WithMessage("Medical Note Id must be greater than zero.");
    }
}
