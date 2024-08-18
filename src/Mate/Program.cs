using System;
using System.IO;

using Specter.Color.Paint;
using Specter.Terminal.Input;
using Specter.Terminal.Output;
using Specter.String;

using Mate.Exceptions;


namespace Mate;


public class MateREPL : DefaultInputStream
{
    public MateREPL() : base()
    {
        Painter = new RulePainter([
            new EqualityRule(156, [
                "+", "-", "*", "/", "(", ")"
            ]),
            new ConditionalRule(215, new LogicCondition(
                LogicCondition.LogicOperation.Or,
                new TokenIsNumber(),
                new TokenIsTarget(".")
            ))
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
        try
        {
            if (args.Length >= 1)
                RunFile(args[0]);
            else
                RunREPL();
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
        => MateLanguage.Run(File.ReadAllText(path));


    public static void RunREPL()
    {
        while (true)
        {
            TerminalStream.Write("> ");
            MateLanguage.Run(new MateREPL().Read());
        }
    }
}