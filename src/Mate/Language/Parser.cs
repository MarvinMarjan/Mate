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
        {
            try
            {
                _statements.Add(Declaration());
            }
            catch (MateException e)
            {
                MateLanguage.LogError(e);

                Synchronize();
                continue;
            }
        }

        return _statements;
    }




    private Statement Declaration() => Peek().Type switch
    {
        TokenType.Var => VarDeclarationStatement(),

        _ => Statement()
    };


    private Statement Statement() => Peek().Type switch
    {
        TokenType.Print => PrintStatement(),

        _ => ExpressionStatement()
    };


    private PrintStatement PrintStatement()
    {
        Advance();
        return new(Expression());
    }


    private VarDeclarationStatement VarDeclarationStatement()
    {
        Advance();

        Token name = Expect(TokenType.Identifier, "Variable name expected.");
        Expect(TokenType.ReceiveOperator, "Receive operator expected.");
        Expression value = Expression();

        return new(name, value);
    }


    private ExpressionStatement ExpressionStatement()
        => new(Expression());








    private Expression Expression()
        => Term();


    private Expression Term()
    {
        Expression expression = SufixFactor();

        while (Match(TokenType.PlusSign, TokenType.MinusSign))
        {
            Token @operator = Previous();
            Expression right = SufixFactor();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression SufixFactor()
    {
        Expression expression = Factor();

        if (Match(TokenType.Identifier))
        {
            Token pseudoToken = Previous() with { Type = TokenType.MultiplicationSign };
            expression = new BinaryExpression(expression, pseudoToken, new IdentifierExpression(Previous()));
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

        if (Match(TokenType.Identifier))
            return new IdentifierExpression(Previous());

        if (Match(TokenType.LeftParen))
        {
            Token start = Previous();
            Expression expression = Expression();

            if (!Match(TokenType.RightParen))
                throw new MateException(new(start), "Unclosed paren.");

            return new GroupingExpression(expression);
        }

        throw new MateException(new (AtEnd() ? Previous() : Peek()), "Expression expected.");
    }


    private void Synchronize()
    {
        Advance();

        while (!AtEnd())
        {
            switch (Peek().Type)
            {
                case TokenType.Print:
                case TokenType.Var:
                    return;
            }

            Advance();
        }
    }


    private Token Expect(TokenType token, string message)
    {
        if (Check(token))
            return Advance();

        throw new MateException(new(Peek()), message);
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