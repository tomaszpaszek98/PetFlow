using Application.Events.Commands.Common;
using Application.Events.Commands.UpdateEvent;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Events.Commands.UpdateEvent;

public class UpdateEventCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1, 2, 3 }
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveValidationErrorWhenIdIsZero()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 0,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1)
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Event Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenIdIsNegative()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = -1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1)
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Event Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenTitleIsEmpty()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = string.Empty,
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1)
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Event title is required.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenTitleExceedsMaxLength()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = new string('A', EventValidatorsConstants.MaxTitleLength + 1),
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1)
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage($"Event title must not exceed {EventValidatorsConstants.MaxTitleLength} characters.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenDescriptionExceedsMaxLength()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = new string('A', EventValidatorsConstants.MaxDescriptionLength + 1),
            DateOfEvent = DateTime.Today.AddDays(1)
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage($"Event description must not exceed {EventValidatorsConstants.MaxDescriptionLength} characters.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenDateOfEventIsInPast()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(-1)
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.DateOfEvent)
            .WithErrorMessage("Event date must be today or in the future.");
    }

    [Test]
    public void ShouldNotHaveValidationErrorWhenDateOfEventIsToday()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveValidationErrorFor(x => x.DateOfEvent);
    }

    [Test]
    public void ShouldHaveValidationErrorWhenPetToAssignIdsContainsNonPositiveId()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1, 0, 3 }
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetToAssignIds)
            .WithErrorMessage("All PetToAssignIds must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenPetToAssignIdsContainsDuplicates()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1, 2, 2 }
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetToAssignIds)
            .WithErrorMessage("PetToAssignIds cannot contain duplicates.");
    }

    [Test]
    public void ShouldNotHaveValidationErrorWhenPetToAssignIdsIsNull()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = null
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveValidationErrorFor(x => x.PetToAssignIds);
    }

    [Test]
    public void ShouldNotHaveValidationErrorWhenPetToAssignIdsIsEmpty()
    {
        // GIVEN
        var validator = new UpdateEventCommandValidator();
        var command = new UpdateEventCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int>()
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveValidationErrorFor(x => x.PetToAssignIds);
    }
}
