using System;
using System.IO;

using Specter.Color.Paint;
using Specter.Terminal.Input;
using Specter.Terminal.Output;
using Specter.String;

using Mate.Exceptions;
using Mate.Language;


namespace Mate;


public class MateREPL : DefaultInputStream
{
    public MateREPL() : base()
    {
        Painter = new RulePainter([
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
            ]) 
        ])
        {
            Cursor = Cursor
        };
    }
}


public class MateProgram
{
    public static void Main(string[] args)
    {
        if (args.Length >= 1)
            RunFile(args[0]);
        else
            RunREPL();
    }


    private static void Run(Action action)
    {
        try
        {
            action();
        }
        catch (MateException e)
        {
            TerminalStream.WriteLine("Error: ".FGBRed() + e.ToString());
        }
        catch (Exception e)
        {
            TerminalStream.WriteLine("Error: ".FGBRed() + e.Message);
        }
    }


    public static void RunFile(string path)
        => Run(() => MateLanguage.Run(File.ReadAllText(path)));


    public static void RunREPL()
    {
        TerminalStream.WriteLine($"Press {"CTRL+C".FGBYellow()} to exit.");

        while (true)
            Run(() => {
                Interpreter interpreter = new();

                while (true)
                {
                    TerminalStream.Write("> ");
                    MateLanguage.Run(new MateREPL().Read(), interpreter);
                }
            });
    }
}