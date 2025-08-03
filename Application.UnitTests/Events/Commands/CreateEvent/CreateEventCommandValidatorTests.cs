using Application.Events.Commands.CreateEvent;

namespace Application.UnitTests.Events.Commands.CreateEvent;

public class CreateEventCommandValidatorTests
{
    [Test]
    public void ShouldReturnValidResultWhenAllFieldsAreValid()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "Event title",
            Description = "Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1, 2, 3 }
        };
        var validator = new CreateEventCommandValidator();
        
        // WHEN
        var result = validator.Validate(command);
        
        // THEN
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void ShouldReturnValidationErrorWhenTitleIsEmpty()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "",
            Description = "Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1 }
        };
        var validator = new CreateEventCommandValidator();
        
        // WHEN
        var result = validator.Validate(command);
        
        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Title");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenTitleIsTooLong()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = new string('A', 51),
            Description = "Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1 }
        };
        var validator = new CreateEventCommandValidator();
        
        // WHEN
        var result = validator.Validate(command);
        
        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Title");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenDescriptionIsTooLong()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "Event title",
            Description = new string('B', 501),
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1 }
        };
        var validator = new CreateEventCommandValidator();
        
        // WHEN
        var result = validator.Validate(command);
        
        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Description");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenDateOfEventIsInThePast()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "Event title",
            Description = "Description",
            DateOfEvent = DateTime.Today.AddDays(-1),
            PetToAssignIds = new List<int> { 1 }
        };
        var validator = new CreateEventCommandValidator();
        
        // WHEN
        var result = validator.Validate(command);
        
        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "DateOfEvent");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenAnyPetToAssignIdIsZeroOrNegative()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "Event title",
            Description = "Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1, 0, -2 }
        };
        var validator = new CreateEventCommandValidator();
        
        // WHEN
        var result = validator.Validate(command);
        
        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage.Contains("greater than zero"));
    }

    [Test]
    public void ShouldReturnValidationErrorWhenPetToAssignIdsHasDuplicates()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "Event title",
            Description = "Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            PetToAssignIds = new List<int> { 1, 2, 2, 3 }
        };
        var validator = new CreateEventCommandValidator();
        
        // WHEN
        var result = validator.Validate(command);
        
        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage.Contains("duplicates"));
    }
}

