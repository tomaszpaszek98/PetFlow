namespace Domain.Exceptions;

public class EntityCreationException : Exception
{
    public EntityCreationException(string message) : base(message)
    {
    }
}