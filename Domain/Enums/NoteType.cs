namespace Domain.Enums;

// This enum is used in presentation layer which is breaking separation,
// but I did it just to avoid creating and mapping new enum. This enum doesn't have
// any business logic, so it is safe to use it in presentation layer.
// It's not the best practice, but it's exceptional, pragmatic approach.
public enum NoteType
{
    Behaviour,
    Mood,
    Symptom,
    General
}