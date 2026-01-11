using MediatR;

namespace Application.MedicalNotes.Commands.DeleteMedicalNote;

public record DeleteMedicalNoteCommand : IRequest
{
    public int PetId { get; init; }
    public int MedicalNoteId { get; init; }
}
