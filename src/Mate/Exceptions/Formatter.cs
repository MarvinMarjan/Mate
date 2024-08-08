using Mate.Language;
using Specter.ANSI;
using Specter.Color;


namespace Mate.Exceptions;


public static class ExceptionFormatter
{
    public static string HighlightTokenRangeFromCurrentSource(TokenRange tokenRange, ColorObject color)
    {
        string result = MateLanguage.CurrentSource;

        result = result.Insert(tokenRange.End.End, EscapeCodes.Reset);
        result = result.Insert(tokenRange.Start.Start, color.AsSequence());

        return result;
    }
}