using MediatR;

namespace Application.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteCommand : IRequest<CreateMedicalNoteResponse>
{
    public int PetId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}