using Domain.Constants;
using FluentValidation;

namespace Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Event Id must be greater than zero.");
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Event title is required.")
            .MaximumLength(EntityConstants.Event.MaxTitleLength)
            .WithMessage($"Event title must not exceed {EntityConstants.Event.MaxTitleLength} characters.");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Event Description is required.")
            .MaximumLength(EntityConstants.Event.MaxDescriptionLength)
            .WithMessage($"Event description must not exceed {EntityConstants.Event.MaxDescriptionLength} characters.");
        
        RuleFor(x => x.DateOfEvent)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Event date must be today or in the future.");
        
        RuleFor(x => x.AssignedPetsIds)
            .Must(AllIdsGreaterThanZero)
            .WithMessage("All PetToAssignIds must be greater than zero.")
            .Must(NoDuplicateIds)
            .WithMessage("PetToAssignIds cannot contain duplicates.");
    }

    private bool AllIdsGreaterThanZero(IEnumerable<int>? ids)
    {
        return ids is null || ids.All(id => id > 0);
    }

    private bool NoDuplicateIds(IEnumerable<int>? petIds)
    {
        if (petIds is null)
            return true;
        
        return petIds.Distinct().Count() == petIds.Count();
    }
}
