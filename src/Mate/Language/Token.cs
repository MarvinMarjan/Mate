using System;
using System.Collections.Generic;

namespace Mate.Language;


public enum TokenType
{
    Invalid,

    PlusSign, MinusSign, MultiplicationSign, DivisionSign,
    LeftParen, RightParen,
    Number,

    Print
}


public readonly struct Token
{
    public static Dictionary<string, TokenType> Keywords { get; } = new([
        new("print", TokenType.Print)
    ]);


    public required string Lexeme { get; init; }
    public required int Start { get; init; }
    public required int End { get; init; }
    public required int Line { get; init; }
    public required object? Value { get; init; }
    public required TokenType Type { get; init; }


    public override string ToString()
        => Lexeme;
}


public readonly struct TokenRange
{
    public Token Start { get; init; }
    public Token End { get; init; }


    public TokenRange(Token start, Token end)
    {
        Start = start;
        End = end;

        if (Start.Line != End.Line)
            throw new ArgumentException("A TokenRange can't store tokens in different lines.");
    }


    public TokenRange(Token token)
        : this(token, token)
    { }
}