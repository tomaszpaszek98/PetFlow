using MediatR;

namespace Application.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteCommand : IRequest<UpdateMedicalNoteResponse>
{
    public int PetId { get; set; }
    public int MedicalNoteId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}