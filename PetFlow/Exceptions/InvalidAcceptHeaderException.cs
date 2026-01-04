namespace PetFlow.Exceptions;

public class InvalidAcceptHeaderException : Exception
{
    public string? ReceivedAcceptHeader { get; }

    public InvalidAcceptHeaderException(string? receivedAcceptHeader)
        : base("Invalid Accept header. API only supports 'Accept: application/json'")
    {
        ReceivedAcceptHeader = receivedAcceptHeader;
    }
}

