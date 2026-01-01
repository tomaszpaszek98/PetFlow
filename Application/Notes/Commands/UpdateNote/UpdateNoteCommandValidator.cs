using Domain.Constants;
using Domain.Enums;
using FluentValidation;

namespace Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
            
        RuleFor(x => x.NoteId)
            .GreaterThan(0)
            .WithMessage("Note Id must be greater than zero.");
            
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(EntityConstants.Note.MaxContentLength)
            .WithMessage($"Content must not exceed {EntityConstants.Note.MaxContentLength} characters.");
            
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be a valid note type. Valid types: " +
                         $"{string.Join(", ", Enum.GetNames<NoteType>())}.");
    }
}
