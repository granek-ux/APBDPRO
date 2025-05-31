using System.Runtime.Serialization;

namespace APBD25_CW11.Exceptions;

public class BadRequestException : Exception
{
    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BadRequestException(string? message) : base(message)
    {
    }

    public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}