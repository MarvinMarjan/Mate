using System;
using System.Text;
using Mate.Language;
using Specter.Color;


namespace Mate.Exceptions;


[Serializable]
public class MateException : Exception
{
    public TokenRange TokenRange { get; init; }


    public MateException()
    { }

    public MateException(TokenRange tokenRange, string message)
        : base(message)
    {
        TokenRange = tokenRange;
    }

    public MateException(TokenRange tokenRange, string message, Exception inner)
        : base(message, inner)
    {
        TokenRange = tokenRange;
    }


    public override string ToString()
    {
        StringBuilder builder = new();

        string highlightedToken = ExceptionFormatter.HighlightTokenRangeFromCurrentSource(TokenRange, ColorObject.FromColor16(Color16.FGRed, null, ColorMode.Underline));

        builder.AppendLine(Message);
        builder.AppendLine("---> " + highlightedToken);

        return builder.ToString();
    }
}