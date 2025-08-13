using Domain.Enums;
using FluentValidation;

namespace Application.Notes.Commands.CreateNote;

public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
            
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(NoteValidatorsConstants.MaxContentLength)
            .WithMessage($"Content must not exceed {NoteValidatorsConstants.MaxContentLength} characters.");
            
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be a valid note type. Valid types: " +
                         $"{string.Join(", ", Enum.GetNames(typeof(NoteType)))}.");
    }
}
