using Application.MedicalNotes.Commands.UpdateMedicalNote;
using Domain.Exceptions;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.MedicalNotes;

public class UpdateMedicalNoteTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldUpdateMedicalNoteWhenNoteAndPetExist()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var medicalNoteId = 1; // MedicalNote from seed data for pet #1
        var updatedTitle = "Updated Medical Note Title";
        var updatedDescription = "Updated description";
        var updateCommand = new UpdateMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = medicalNoteId,
            Title = updatedTitle,
            Description = updatedDescription
        };

        // WHEN
        var result = await Sender.Send(updateCommand);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(medicalNoteId);
        result.Title.Should().Be(updatedTitle);
        result.Description.Should().Be(updatedDescription);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotExist()
    {
        // GIVEN
        var petId = 1;
        var nonExistentNoteId = 999999;
        var updateCommand = new UpdateMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = nonExistentNoteId,
            Title = "Title",
            Description = "Description"
        };

        // WHEN
        var act = async () => await Sender.Send(updateCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*MedicalNote*{nonExistentNoteId}*");
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var medicalNoteId = 1;
        var updateCommand = new UpdateMedicalNoteCommand
        {
            PetId = nonExistentPetId,
            MedicalNoteId = medicalNoteId,
            Title = "Title",
            Description = "Description"
        };

        // WHEN
        var act = async () => await Sender.Send(updateCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Pet*{nonExistentPetId}*");
    }

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var petId = 2; // Pet from seed data
        var medicalNoteId = 2; // MedicalNote from seed data for pet #2
        var invalidUpdateCommand = new UpdateMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = medicalNoteId,
            Title = string.Empty,
            Description = "Description"
        };

        // WHEN
        var act = async () => await Sender.Send(invalidUpdateCommand);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }
}

