using System.Collections.Generic;

using Mate.Language;


namespace Mate;


public static class MateLanguage
{
    public static string[] CurrentSource { get; set; } = [];


    public static void Run(string source)
    {
        CurrentSource = source.Split('\n');

        List<Token> tokens = new Scanner().Scan(source);
        List<Statement> statements = new Parser().Parse(tokens);
        new Interpreter().Interpret(statements);
    }
}