using System;
using System.Collections.Generic;

using Specter.Terminal.Output;


namespace Mate.Language;


public class Interpreter : IExpressionProcessor<double>, IStatementProcessor<object?>
{
    public void Interpret(Statement statement)
        => statement.Process(this);

    public void Interpret(List<Statement> statements)
    {
        foreach (Statement statement in statements)
            statement.Process(this);
    }

    public double Interpret(Expression expression)
        => expression.Process(this);




    public object? ProcessExpressionStatement(ExpressionStatement statement)
    {
        Interpret(statement.Expression);
        return null;
    }

    public object? ProcessPrintStatement(PrintStatement statement)
    {
        TerminalStream.WriteLine(Interpret(statement.Value).SingleResultToString());
        return null;
    }




    public double ProcessLiteralExpression(LiteralExpression expression)
        => expression.Value is double number ? number : throw new ArgumentException("Literal is not a number.");

    public double ProcessBinaryExpression(BinaryExpression expression)
    {
        double left = Interpret(expression.Left);
        double right = Interpret(expression.Right);
    
        return expression.Operator.Type switch
        {
            TokenType.PlusSign => left + right,
            TokenType.MinusSign => left - right,
            TokenType.MultiplicationSign => left * right,
            TokenType.DivisionSign => left / right,

            _ => throw new InvalidOperationException("Invalid binary operation.")
        };
    }

    public double ProcessUnaryExpression(UnaryExpression expression)
    {
        double right = Interpret(expression.Right);

        return expression.Operator.Type switch
        {
            TokenType.MinusSign => -right,

            _ => throw new InvalidOperationException("Invalid unary operation.")
        };
    }
    
    public double ProcessGroupingExpression(GroupingExpression expression)
        => Interpret(expression.Expression);
}