using FluentValidation;

namespace Application.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteCommandValidator : AbstractValidator<CreateMedicalNoteCommand>
{
    public CreateMedicalNoteCommandValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
            
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(MedicalNoteValidatorsConstants.MaxTitleLength)
            .WithMessage($"Title must not exceed {MedicalNoteValidatorsConstants.MaxTitleLength} characters.");
            
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(MedicalNoteValidatorsConstants.MaxDescriptionLength)
            .WithMessage($"Description must not exceed {MedicalNoteValidatorsConstants.MaxDescriptionLength} characters.");
    }
}
