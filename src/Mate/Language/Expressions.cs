namespace Mate.Language;


public interface IExpressionProcessor<T>
{
    public T ProcessLiteralExpression(LiteralExpression expression);
    public T ProcessBinaryExpression(BinaryExpression expression);
    public T ProcessUnaryExpression(UnaryExpression expression);
    public T ProcessGroupingExpression(GroupingExpression expression);
}


public abstract class Expression
{
    public abstract T Process<T>(IExpressionProcessor<T> processor);
}


public class LiteralExpression(object value) : Expression
{
    public object Value { get; init; } = value;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessLiteralExpression(this);
}


public class BinaryExpression(Expression left, Token @operator, Expression right) : Expression
{
    public Expression Left { get; init; } = left;
    public Token Operator { get; init; } = @operator;
    public Expression Right { get; init; } = right;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessBinaryExpression(this);
}


public class UnaryExpression(Token @operator, Expression right) : Expression
{
    public Token Operator { get; init; } = @operator;
    public Expression Right { get; init; } = right;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessUnaryExpression(this);
}


public class GroupingExpression(Expression expression) : Expression
{
    public Expression Expression { get; init; } = expression;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessGroupingExpression(this);
}