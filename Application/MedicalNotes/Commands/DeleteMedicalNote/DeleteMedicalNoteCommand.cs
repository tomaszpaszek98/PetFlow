using MediatR;

namespace Application.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommand : IRequest
{
    public int PetId { get; set; }
    public int MedicalNoteId { get; set; }
}
