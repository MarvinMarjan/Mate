using Specter.Color.Paint;
using Specter.Terminal.Input;
using Specter.Terminal.Output;
using Specter.String;

using Mate.Language;
using Mate.Exceptions;
using System;


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
    public static void Main()
    {
        while (true)
        {
            try
            {
                TerminalStream.Write("> ");
                string input = new MateREPL().Read();
    
                MateLanguage.Run(input);
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
    }
}