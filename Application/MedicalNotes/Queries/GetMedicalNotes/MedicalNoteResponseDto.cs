﻿namespace Application.MedicalNotes.Queries.GetMedicalNotes;

public class MedicalNoteResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}