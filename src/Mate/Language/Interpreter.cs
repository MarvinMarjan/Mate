using System;
using System.Collections.Generic;

using Specter.Terminal.Output;

using Mate.Exceptions;
using Mate.Language.Calculation;


namespace Mate.Language;


public class Interpreter : IExpressionProcessor<double>, IStatementProcessor<object?>
{
    public Dictionary<string, double> Variables { get; init; } = new([
        new("k", 1_000),
        new("m", 1_000_000),
        new("b", 1_000_000_000)
    ]);


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

    public object? ProcessVarDeclarationStatement(VarDeclarationStatement statement)
    {
        Variables.Add(statement.Name.Lexeme, Interpret(statement.Value));
        return null;
    }




    public double ProcessLiteralExpression(LiteralExpression expression)
        => expression.Value is double number ? number : throw new ArgumentException("Literal is not a number.");

    public double ProcessIdentifierExpression(IdentifierExpression expression)
    {
        string identifierName = expression.Identifier.Lexeme;

        if (!Variables.TryGetValue(identifierName, out double value))
            throw new MateException(new(expression.Identifier), "Undefined identifier.");

        return value;
    }

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


    public double ProcessFractionExpression(FractionExpression expression)
    {
        double numerator = Interpret(expression.Numerator);
        double denominator = Interpret(expression.Denominator);

        return numerator / denominator;
    }
}