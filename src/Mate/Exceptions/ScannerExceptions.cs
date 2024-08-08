using System;

using Mate.Language;


namespace Mate.Exceptions;


[Serializable]
public class InvalidTokenException : MateException
{
    public InvalidTokenException()
    { }

    public InvalidTokenException(TokenRange tokenRange, string message)
        : base(tokenRange, message)
    { }

    public InvalidTokenException(TokenRange tokenRange, string message, Exception inner)
        : base(tokenRange, message, inner)
    { }
}