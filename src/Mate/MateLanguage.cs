using System.Collections.Generic;

using Specter.Terminal.Output;

using Mate.Language;


namespace Mate;


public static class MateLanguage
{
    public static string CurrentSource { get; set; } = "";


    public static void Run(string source)
    {
        CurrentSource = source;

        List<Token> tokens = new Scanner().Scan(source);
        Expression expression = new Parser().Parse(tokens);
        double result = new Interpreter().Interpret(expression);

        TerminalStream.WriteLine(ResultFormatter.SingleResultToString(result));
    }
}