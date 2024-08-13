using System.Globalization;
using System.Text;

namespace Mate.Language;


public class ASTPrinter : IExpressionProcessor<string>
{
    public string Print(Expression expression)
        => expression.Process(this);


    public string ProcessLiteralExpression(LiteralExpression expression)
    {
        // uses '.' as separator instead of ','
        if (expression.Value is double number)
            return number.ToString(CultureInfo.InvariantCulture);

        return expression.Value.ToString() ?? "invalid";
    }

    public string ProcessBinaryExpression(BinaryExpression expression)
        => Stringify(expression.Operator.Lexeme, expression.Left, expression.Right);

    public string ProcessUnaryExpression(UnaryExpression expression)
        => Stringify(expression.Operator.Lexeme, expression.Right);

    public string ProcessGroupingExpression(GroupingExpression expression)
        => Stringify("group", expression.Expression);


    private string Stringify(string name, params Expression[] expressions)
    {
        StringBuilder builder = new();

        builder.Append($"({name}");

        foreach (Expression expression in expressions)
        {
            builder.Append(' ');
            builder.Append(expression.Process(this));
        }

        builder.Append(')');

        return builder.ToString();
    }
}