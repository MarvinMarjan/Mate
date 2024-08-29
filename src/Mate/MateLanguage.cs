using System;
using System.Collections.Generic;

using Specter.String;
using Specter.Terminal.Output;
using Specter.Color;
using Specter.Color.Paint;

using Mate.Language;
using Mate.Exceptions;

using Token = Mate.Language.Token;


namespace Mate;


public static class MateLanguage
{
    public static Interpreter CurrentInterpreter { get; private set; } = new();

    public static string[] CurrentSource { get; set; } = [];
    public static bool Failed { get; private set; }

    public static RulePainter GlobalPainter { get; set; } = new RulePainter([
        new EqualityRule(156, [
            "+", "-", "*", "/", "(", ")", ":", "="
        ]),
        new ConditionalRule(215, new LogicCondition(
            LogicCondition.LogicOperation.Or,
            new TokenIsNumber(),
            new TokenIsTarget(".")
        )),
        new EqualityRule(111, [
            "print", "var"
        ]),
        new BetweenRule(ColorObject.FromColor256(241, mode: ColorMode.Italic), "#", new("\n") {ShouldIgnoreWhitespace = false}),
        new BetweenRule(ColorObject.FromColor256(241, mode: ColorMode.Italic),
            new("#", "<"), new(">", "#")
        )
    ]);


    public static void Run(string source, Interpreter? interpreter = null)
    {
        CurrentInterpreter = new();
        CurrentSource = source.Split('\n');

        List<Token> tokens = new Scanner().Scan(source);

        if (tokens.Count == 0)
            Environment.Exit(0);

        List<Statement> statements = new Parser().Parse(tokens);
        
        if (!Failed)
            CurrentInterpreter.Interpret(statements);
    }


    public static void LogError(Exception exception)
    {
        TerminalStream.WriteLine("Error: ".FGBRed() + (exception is MateException ? exception.ToString() : exception.Message));
        Failed = true;
    }
}