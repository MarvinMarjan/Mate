using System;
using System.Text;

using Specter.Color;

using Mate.Language;


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

        string arrow = ExceptionFormatter.GenerateHighlightArrowFromRange(TokenRange, ColorObject.FromColor16(Color16.FGRed));
        string line = MateLanguage.CurrentSource[TokenRange.Start.Line - 1];

        string lineNumber = $"{TokenRange.Start.Line}. ";
        arrow = new string(' ', lineNumber.Length) + arrow;
    
        builder.AppendLine(Message);
        builder.AppendLine($"{lineNumber}{MateLanguage.GlobalPainter.Paint(line)}\n{arrow}");

        return builder.ToString();
    }
}