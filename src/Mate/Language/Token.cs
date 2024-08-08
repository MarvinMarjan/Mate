namespace Mate.Language;


public enum TokenType
{
    Invalid,

    PlusSign, MinusSign, MultiplicationSign, DivisionSign,
    LeftParen, RightParen,
    Number
}


public readonly struct Token
{
    public required string Lexeme { get; init; }
    public required int Start { get; init; }
    public required int End { get; init; }
    public required object? Value { get; init; }
    public required TokenType Type { get; init; }


    public override string ToString()
        => Lexeme;
}


public readonly struct TokenRange(Token start, Token end)
{
    public Token Start { get; init; } = start;
    public Token End { get; init; } = end;


    public TokenRange(Token token)
        : this(token, token)
    { }
}