using Specter.ANSI;
using Specter.Color;

using Mate.Language;


namespace Mate.Exceptions;


public static class ExceptionFormatter
{
    public static string HighlightTokenRangeFromCurrentSource(TokenRange tokenRange, ColorObject color)
    {
        string[] languageSource = MateLanguage.CurrentSource;
        string line = languageSource[tokenRange.Start.Line - 1];

        line = line.Insert(tokenRange.End.End, EscapeCodes.Reset);
        line = line.Insert(tokenRange.Start.Start, color.AsSequence());

        return line;
    }
}