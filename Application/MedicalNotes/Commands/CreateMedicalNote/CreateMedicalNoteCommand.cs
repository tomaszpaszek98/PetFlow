using MediatR;

namespace Application.MedicalNotes.Commands.CreateMedicalNote;

public record CreateMedicalNoteCommand : IRequest<CreateMedicalNoteResponse>
{
    public int PetId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}