using Application.MedicalNotes.Commands.DeleteMedicalNote;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.MedicalNotes;

public class DeleteMedicalNoteTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldDeleteMedicalNoteWhenNoteAndPetExist()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var medicalNoteId = 1; // MedicalNote from seed data for pet #1
        var deleteCommand = new DeleteMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = medicalNoteId
        };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotExist()
    {
        // GIVEN
        var petId = 1;
        var nonExistentNoteId = 999999;
        var deleteCommand = new DeleteMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = nonExistentNoteId
        };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

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
        var deleteCommand = new DeleteMedicalNoteCommand
        {
            PetId = nonExistentPetId,
            MedicalNoteId = medicalNoteId
        };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Pet*{nonExistentPetId}*");
    }
}

