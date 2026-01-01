using Application.MedicalNotes.Commands;
using Domain.Constants;
using FluentValidation;

namespace Application.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteCommandValidator : AbstractValidator<UpdateMedicalNoteCommand>
{
    public UpdateMedicalNoteCommandValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
            
        RuleFor(x => x.MedicalNoteId)
            .GreaterThan(0)
            .WithMessage("Medical Note Id must be greater than zero.");
            
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(EntityConstants.MedicalNote.MaxTitleLength)
            .WithMessage($"Title must not exceed {EntityConstants.MedicalNote.MaxTitleLength} characters.");
            
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(EntityConstants.MedicalNote.MaxDescriptionLength)
            .WithMessage($"Description must not exceed {EntityConstants.MedicalNote.MaxDescriptionLength} characters.");
    }
}
