using System.Collections.Generic;

using Specter.Terminal.Output;


namespace Mate.Language;


public static class MateLanguage
{
    public static string CurrentSource { get; set; } = "";


    public static void Run(string source)
    {
        CurrentSource = source;

        List<Token> tokens = new Scanner().Scan(source);
        Expression expression = new Parser().Parse(tokens);

        TerminalStream.WriteLine(new ASTPrinter().Print(expression));
    }
}