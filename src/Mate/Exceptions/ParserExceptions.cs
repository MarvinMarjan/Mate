using System;

using Mate.Language;


namespace Mate.Exceptions;


[Serializable]
public class ExpressionExpectedException : MateException
{
    public ExpressionExpectedException()
    { }

    public ExpressionExpectedException(TokenRange tokenRange, string message)
        : base(tokenRange, message)
    { }

    public ExpressionExpectedException(TokenRange tokenRange, string message, Exception inner)
        : base(tokenRange, message, inner)
    { }
}


[Serializable]
public class UnclosedParensException : MateException
{
    public UnclosedParensException()
    { }

    public UnclosedParensException(TokenRange tokenRange, string message)
        : base(tokenRange, message)
    { }

    public UnclosedParensException(TokenRange tokenRange, string message, Exception inner)
        : base(tokenRange, message, inner)
    { }
}