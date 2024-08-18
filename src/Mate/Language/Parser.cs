using System;
using System.Collections.Generic;

using Mate.Exceptions;


namespace Mate.Language;


public class Parser
{
    private List<Token> _tokens = [];
    private List<Statement> _statements = [];
    private int _current = 0;


    public List<Statement> Parse(List<Token> tokens)
    {
        _tokens = tokens;
        _statements = [];
        _current = 0;

        if (_tokens.Count == 0)
            throw new ArgumentException("Don't parse a empty token list.");

        while (!AtEnd())
            _statements.Add(Statement());

        return _statements;
    }


    private Statement Statement()
    {
        TokenType token = Peek().Type;

        if (Token.Keywords.ContainsValue(token))
            Advance();

        return token switch
        {
            TokenType.Print => PrintStatement(),

            _ => ExpressionStatement()
        };
    }


    private PrintStatement PrintStatement()
        => new(Expression());


    private ExpressionStatement ExpressionStatement()
        => new(Expression());




    private Expression Expression()
        => Term();


    private Expression Term()
    {
        Expression expression = Factor();

        while (Match(TokenType.PlusSign, TokenType.MinusSign))
        {
            Token @operator = Previous();
            Expression right = Factor();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression Factor()
    {
        Expression expression = Unary();

        while (Match(TokenType.MultiplicationSign, TokenType.DivisionSign))
        {
            Token @operator = Previous();
            Expression right = Unary();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression Unary()
    {
        if (Match(TokenType.MinusSign))
        {
            Token @operator = Previous();
            Expression right = Unary();
            return new UnaryExpression(@operator, right);
        }

        return Primary();
    }


    private Expression Primary()
    {
        if (Match(TokenType.Number))
            return new LiteralExpression(Previous().Value!);

        if (Match(TokenType.LeftParen))
        {
            Token start = Previous();
            Expression expression = Expression();

            if (!Match(TokenType.RightParen))
                throw new UnclosedParensException(new(start), "Unclosed paren.");

            return new GroupingExpression(expression);
        }

        throw new ExpressionExpectedException(new (AtEnd() ? Previous() : Peek()), "Expression expected.");
    }


    private bool Match(params TokenType[] tokens)
    {
        foreach (TokenType token in tokens)
        {
            if (!Check(token))
                continue;

            Advance();
            return true;
        }

        return false;
    }

    private bool Check(TokenType tokenType)
    {
        if (AtEnd())
            return false;

        return Peek().Type == tokenType;
    }

    private bool AtEnd()
        => _current >= _tokens.Count;
    

    private Token Advance()
    {
        if (!AtEnd())
            _current++;
        
        return Previous();
    }

    private Token Peek()
        => _tokens[_current];

    private Token Previous()
        => _tokens[_current - 1];

}