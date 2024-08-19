using System;
using System.IO;

using Specter.Terminal.Input;
using Specter.Terminal.Output;
using Specter.String;

using Mate.Language;


namespace Mate;


public class MateREPL : DefaultInputStream
{
    public MateREPL() : base()
    {
        Painter = MateLanguage.GlobalPainter;
        Painter.Cursor = Cursor;
    }
}


// TODO: add notation for representing large numbers:

// 1000000 = 1_000_000 or 1mi or 1000m (there's also 1bi)

// TODO: add first-degree equation solution
// TODO: add support for the following notation:

// var x = 10
// print 2x # 2 * 10

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
        catch (Exception e)
        {
            MateLanguage.LogError(e);
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