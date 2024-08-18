using System.Globalization;
using System.Collections.Generic;

using Mate.Exceptions;


namespace Mate.Language;


public class Scanner
{
    private string _source = "";
    private int _start, _end;
    private int _relativeStart, _relativeEnd;
    private int _line;

    private List<Token> _tokens = [];


    public List<Token> Scan(string source)
    {
        _source = source;
        _start = _end = 0;
        _relativeStart = _relativeEnd = 0;
        _line = 1;

        _tokens = [];

        while (!AtEnd())
        {
            _start = _end;
            _relativeStart = _relativeEnd;
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
            break;

        case '\n':
            _line++;
            _relativeStart = _relativeEnd = 0;
            break;

        case '+': AddToken(TokenType.PlusSign); break;
        case '-': AddToken(TokenType.MinusSign); break;
        case '*': AddToken(TokenType.MultiplicationSign); break;
        case '/': AddToken(TokenType.DivisionSign); break;

        case '(': AddToken(TokenType.LeftParen); break;
        case ')': AddToken(TokenType.RightParen); break;

        default:
            if (char.IsLetter(ch))
                Identifier();

            else if (char.IsDigit(ch))
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
            Start = _relativeStart,
            End = _relativeEnd,
            Line = _line,
            Value = value,
            Type = tokenType
        };


    private void Identifier()
    {
        while (char.IsLetterOrDigit(Peek()))
            Advance();

        if (Token.Keywords.TryGetValue(CurrentSubstring(), out TokenType keyword))
            AddToken(keyword);
        else
            AddToken(TokenType.Identifier);
    }


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

    private char Advance()
    {
        if (!AtEnd())
        {
            _relativeEnd++;
            return _source[_end++];
        }

        return '\0';
    }

    private char Peek() => !AtEnd() ? _source[_end] : '\0';
    private char Next() => _source[_end + 1];
}