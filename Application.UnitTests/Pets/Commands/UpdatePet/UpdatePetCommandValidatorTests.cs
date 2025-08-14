using Application.Pets.Commands.UpdatePet;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Pets.Commands.UpdatePet;

public class UpdatePetCommandValidatorTests
{
    [Test]
    public void ShouldReturnValidationSuccessWhenAllFieldsAreValid()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Denny",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldReturnValidationErrorWhenNameIsEmpty()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldReturnValidationErrorWhenSpeciesIsEmpty()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Denny",
            Species = "",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Species);
    }

    [Test]
    public void ShouldReturnValidationErrorWhenBreedIsEmpty()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Denny",
            Species = "Dog",
            Breed = "",
            DateOfBirth = DateTime.Today
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Breed);
    }

    [Test]
    public void ShouldReturnValidationErrorWhenDateOfBirthIsInTheFuture()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Denny",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today.AddDays(1)
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Test]
    public void ShouldReturnValidationErrorWhenNameIsTooLong()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = new string('A', 51),
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldReturnValidationErrorWhenSpeciesIsTooLong()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Denny",
            Species = new string('B', 31),
            Breed = "Chihuahua",
            DateOfBirth = DateTime.Today
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Species);
    }

    [Test]
    public void ShouldReturnValidationErrorWhenBreedIsTooLong()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Denny",
            Species = "Dog",
            Breed = new string('C', 31),
            DateOfBirth = DateTime.Today
        };
        var validator = new UpdatePetCommandValidator();

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Breed);
    }
}
