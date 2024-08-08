using System.Collections.Generic;


namespace Mate.Language;


public static class MateLanguage
{
    public static string CurrentSource { get; set; } = "";


    public static void Run(string source)
    {
        CurrentSource = source;

        List<Token> tokens = new Scanner().Scan(source);
    }
}