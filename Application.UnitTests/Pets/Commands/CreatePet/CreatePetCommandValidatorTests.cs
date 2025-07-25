using Application.Pets.Commands.CreatePet;

namespace Application.UnitTests.Pets.Commands.CreatePet;

public class CreatePetCommandValidatorTests
{
    [Test]
    public void ShouldReturnValidationSuccessWhenAllFieldsAreValid()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "Denny",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void ShouldReturnValidationErrorWhenNameIsEmpty()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Name");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenSpeciesIsEmpty()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "Denny",
            Species = "",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Species");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenBreedIsEmpty()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "Denny",
            Species = "Dog",
            Breed = "",
            DateOfBirth = DateTime.Today
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Breed");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenDateOfBirthIsInTheFuture()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "Denny",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today.AddDays(1)
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "DateOfBirth");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenNameIsTooLong()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = new string('A', 51),
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Name");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenSpeciesIsTooLong()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "Denny",
            Species = new string('B', 31),
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Species");
    }

    [Test]
    public void ShouldReturnValidationErrorWhenBreedIsTooLong()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "Denny",
            Species = "Dog",
            Breed = new string('C', 31),
            DateOfBirth = DateTime.Today
        };
        var validator = new CreatePetCommandValidator();

        // WHEN
        var result = validator.Validate(command);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Breed");
    }
}
