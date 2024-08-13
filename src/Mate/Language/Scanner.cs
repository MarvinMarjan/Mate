using System.Collections.Generic;
using System.Globalization;

using Mate.Exceptions;


namespace Mate.Language;


public class Scanner
{
    private string _source = "";
    private int _start, _end;

    private List<Token> _tokens = [];


    public List<Token> Scan(string source)
    {
        _source = source;
        _start = _end = 0;

        _tokens = [];

        while (!AtEnd())
        {
            _start = _end;
            ScanToken();
        }

        return _tokens;
    }


    private void ScanToken()
    {
        char ch = Advance();

        switch (ch)
        {
        case ' ':
        case '\t':
        case '\n':
            break;

        case '+': AddToken(TokenType.PlusSign); break;
        case '-': AddToken(TokenType.MinusSign); break;
        case '*': AddToken(TokenType.MultiplicationSign); break;
        case '/': AddToken(TokenType.DivisionSign); break;

        case '(': AddToken(TokenType.LeftParen); break;
        case ')': AddToken(TokenType.RightParen); break;

        default:
            if (char.IsDigit(ch))
                Number();
            
            else
                throw new InvalidTokenException(new(TokenFromCurrent(TokenType.Invalid)), "Invalid token.");

            break;
        }
    }


    private void AddToken(TokenType tokenType, object? value = null)
        => _tokens.Add(TokenFromCurrent(tokenType, value));

    private Token TokenFromCurrent(TokenType tokenType, object? value = null)
        => new() {
            Lexeme =  CurrentSubstring(),
            Start = _start,
            End = _end,
            Value = value,
            Type = tokenType
        };


    private void Number()
    {
        AdvanceUntilNonDigit();

        if (Peek() == '.')
        {
            Advance();
            AdvanceUntilNonDigit();
        }

        _ = double.TryParse(CurrentSubstring(), CultureInfo.InvariantCulture, out double value);

        AddToken(TokenType.Number, value);
    }

    private void AdvanceUntilNonDigit()
    {
        while (char.IsDigit(Peek()))
            Advance();
    }


    private string CurrentSubstring()
        => _source[_start .. _end];

    private bool AtEnd() => _end >= _source.Length;

    private char Advance() => !AtEnd() ? _source[_end++] : '\0';
    private char Peek() => !AtEnd() ? _source[_end] : '\0';
    private char Next() => _source[_end + 1];
}