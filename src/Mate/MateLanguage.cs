using System;
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

        if (tokens.Count == 0)
            Environment.Exit(0);

        List<Statement> statements = new Parser().Parse(tokens);
        new Interpreter().Interpret(statements);
    }
}