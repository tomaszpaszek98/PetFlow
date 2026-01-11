using Application.MedicalNotes.Commands.CreateMedicalNote;
using Application.MedicalNotes.Commands.UpdateMedicalNote;
using Application.MedicalNotes.Queries.GetMedicalNoteDetails;
using Application.MedicalNotes.Queries.GetMedicalNotes;
using Domain.Entities;

namespace Application.MedicalNotes;

public static class MappingExtensions
{
    public static MedicalNote MapToMedicalNote(this CreateMedicalNoteCommand command)
    {
        return new MedicalNote
        {
            PetId = command.PetId,
            Title = command.Title,
            Description = command.Description
        };
    }

    public static CreateMedicalNoteResponse MapToCreateResponse(this MedicalNote medicalNote)
    {
        return new CreateMedicalNoteResponse
        {
            Id = medicalNote.Id,
            Title = medicalNote.Title,
            Description = medicalNote.Description,
            CreatedAt = medicalNote.Created
        };
    }

    public static MedicalNoteDetailsResponse MapToDetailsResponse(this MedicalNote medicalNote)
    {
        return new MedicalNoteDetailsResponse
        {
            Id = medicalNote.Id,
            Title = medicalNote.Title,
            Description = medicalNote.Description,
            CreatedAt = medicalNote.Created,
            ModifiedAt = medicalNote.Modified ?? medicalNote.Created
        };
    }

    public static MedicalNotesResponse MapToMedicalNotesResponse(this IEnumerable<MedicalNote> medicalNotes)
    {
        return new MedicalNotesResponse
        {
            Items = medicalNotes.Select(MapToMedicalNoteDto)
        };
    }

    private static MedicalNoteResponseDto MapToMedicalNoteDto(this MedicalNote medicalNote)
    {
        return new MedicalNoteResponseDto
        {
            Id = medicalNote.Id,
            Title = medicalNote.Title,
            CreatedAt = medicalNote.Created,
            ModifiedAt = medicalNote.Modified ?? medicalNote.Created
        };
    }

    public static UpdateMedicalNoteResponse MapToUpdateResponse(this MedicalNote medicalNote)
    {
        return new UpdateMedicalNoteResponse
        {
            Id = medicalNote.Id,
            Title = medicalNote.Title,
            Description = medicalNote.Description,
            CreatedAt = medicalNote.Created,
            ModifiedAt = medicalNote.Modified ?? medicalNote.Created
        };
    }
}