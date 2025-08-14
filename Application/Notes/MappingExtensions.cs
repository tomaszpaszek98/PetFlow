using Application.Notes.Commands.CreateNote;
using Application.Notes.Commands.UpdateNote;
using Application.Notes.Queries.GetNoteDetails;
using Application.Notes.Queries.GetNotes;
using Domain.Entities;

namespace Application.Notes;

public static class MappingExtensions
{
    public static Note MapToNote(this CreateNoteCommand command)
    {
        return new Note
        {
            PetId = command.PetId,
            Content = command.Content,
            Type = command.Type
        };
    }

    public static CreateNoteResponse MapToCreateResponse(this Note note)
    {
        return new CreateNoteResponse
        {
            Id = note.Id,
            Content = note.Content,
            Type = note.Type,
            CreatedAt = note.Created
        };
    }
    
    public static NoteDetailsResponse MapToDetailsResponse(this Note note)
    {
        return new NoteDetailsResponse
        {
            Id = note.Id,
            Content = note.Content,
            Type = note.Type,
            CreatedAt = note.Created,
            ModifiedAt = note.Modified ?? note.Created
        };
    }

    public static NotesResponse MapToNotesResponse(this IEnumerable<Note> notes)
    {
        return new NotesResponse
        {
            Notes = notes.Select(MapToNoteDto)
        };
    }
    
    private static NoteResponseDto MapToNoteDto(Note note)
    {
        return new NoteResponseDto
        {
            Id = note.Id,
            Type = note.Type,
            ShortContent = TruncateContent(note.Content),
            CreatedAt = note.Created,
            ModifiedAt = note.Modified ?? note.Created
        };
    }
    
    private static string TruncateContent(string content, int maxLength = 100)
    {
        return content.Length > maxLength 
            ? content.Substring(0, maxLength - 3) + "..." 
            : content;
    }

    public static UpdateNoteResponse MapToUpdateResponse(this Note note)
    {
        return new UpdateNoteResponse
        {
            Id = note.Id,
            Content = note.Content,
            Type = note.Type,
            CreatedAt = note.Created,
            ModifiedAt = note.Modified ?? note.Created
        };
    }
}