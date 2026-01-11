using MediatR;

namespace Application.MedicalNotes.Commands.UpdateMedicalNote;

public record UpdateMedicalNoteCommand : IRequest<UpdateMedicalNoteResponse>
{
    public int PetId { get; init; }
    public int MedicalNoteId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}