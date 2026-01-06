using Application.MedicalNotes.Queries.GetMedicalNoteDetails;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.MedicalNotes;

public class GetMedicalNoteDetailsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnMedicalNoteDetailsWhenNoteExists()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var medicalNoteId = 1; // MedicalNote from seed data for pet #1
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = petId,
            MedicalNoteId = medicalNoteId
        };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Id.Should().Be(medicalNoteId);
        response.Title.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var petId = 1;
        var nonExistentNoteId = 999999;
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = petId,
            MedicalNoteId = nonExistentNoteId
        };

        // WHEN
        var act = async () => await Sender.Send(query);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*MedicalNote*{nonExistentNoteId}*");
    }
}

