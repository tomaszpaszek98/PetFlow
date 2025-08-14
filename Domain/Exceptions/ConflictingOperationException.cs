namespace Domain.Exceptions;

public class ConflictingOperationException : Exception
{
    public ConflictingOperationException(string message) : base(message)
    {
    }
}