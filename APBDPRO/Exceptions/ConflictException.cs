﻿using System.Runtime.Serialization;

namespace APBDPRO.Exceptions;

public class ConflictException : Exception
{
    protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ConflictException(string? message) : base(message)
    {
    }

    public ConflictException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}