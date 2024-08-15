using System;


namespace Mate.Language;


public class Interpreter : IExpressionProcessor<double>
{
    public double Interpret(Expression expression)
        => expression.Process(this);


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