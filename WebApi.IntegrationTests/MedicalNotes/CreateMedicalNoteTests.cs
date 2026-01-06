using Application.MedicalNotes.Commands.CreateMedicalNote;
using Domain.Exceptions;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.MedicalNotes;

public class CreateMedicalNoteTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldCreateMedicalNoteWhenPetExists()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var title = "New Medical Note";
        var description = "Test medical note description";
        var command = new CreateMedicalNoteCommand
        {
            PetId = petId,
            Title = title,
            Description = description
        };

        // WHEN
        var result = await Sender.Send(command);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be(title);
        result.Description.Should().Be(description);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var command = new CreateMedicalNoteCommand
        {
            PetId = nonExistentPetId,
            Title = "Medical Note",
            Description = "Description"
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Pet*{nonExistentPetId}*");
    }

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var command = new CreateMedicalNoteCommand
        {
            PetId = petId,
            Title = string.Empty,
            Description = "Description"
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }
}

