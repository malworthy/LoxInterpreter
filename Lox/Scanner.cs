using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
public class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = new();
    private int start = 0;
    private int current = 0;
    private int line = 1;
    private readonly Dictionary<string, TokenType> keywords;

    private bool AtEnd { get => current >= source.Length; }

    public Scanner(string source)
    {
        this.source = source;
        keywords = new Dictionary<string, TokenType>();
        keywords["and"] = TokenType.AND;
        keywords["class"] = TokenType.CLASS;
        keywords["else"] = TokenType.ELSE;
        keywords["false"] = TokenType.FALSE;
        keywords["true"] = TokenType.TRUE;
        keywords["for"] = TokenType.FOR;

        keywords["fun"] = TokenType.FUN;
        keywords["if"] = TokenType.IF;
        keywords["nil"] = TokenType.NIL;
        keywords["or"] = TokenType.OR;
        keywords["print"] = TokenType.PRINT;
        keywords["return"] = TokenType.RETURN;
        keywords["super"] = TokenType.SUPER;
        keywords["this"] = TokenType.THIS;
        keywords["var"] = TokenType.VAR;
        keywords["while"] = TokenType.WHILE;
    }

    public List<Token> ScanTokens()
    {
        while(!AtEnd)
        {
            start = current;
            ScanToken();
        }
        tokens.Add(new Token { Type = TokenType.EOF, Line = line });

        return tokens;
    }

    private void ScanToken()
    {
        const string tokens = "(){},.-+;*";
        char c = Advance();
        var index = tokens.IndexOf(c);
        if (index >= 0)
        {
            AddToken((TokenType)index);
        }
        else if (char.IsWhiteSpace(c) && c != '\n')
        {
            // ignore
        }
        else if (char.IsDigit(c))
        {
            ParseNumber();
        }
        else if (IsAlpha(c))
        {
            Identifier();
        }
        else
        {
            switch(c)
            {
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (Match('/'))
                        while (Peek() != '\n' && !AtEnd)
                            Advance();
                    else
                        AddToken(TokenType.SLASH);
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    ParseString();
                    break;
                default:
                    Lox.Error(line, "Unexpected character.");
                    break;
            }
        }
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();

        var text = source[start..current];
        var type = TokenType.IDENTIFIER;

        if (keywords.ContainsKey(text))
            type = keywords[text];

        AddToken(type);
    }

    private bool IsAlpha(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    private bool IsAlphaNumeric(char c) => IsAlpha(c) || char.IsNumber(c);
    private void ParseNumber()
    {
        while (char.IsDigit(Peek())) Advance();

        // Look for a fractional part.
        if (Peek() == '.' && char.IsDigit(PeekNext()))
        {
            // Consume the "."
            Advance();

            while (char.IsDigit(Peek())) Advance();
        }
        if (!decimal.TryParse(source[start..current], out var result))
        {
            Lox.Error(line, "Not a valid number (too large or too small)");
        }
        AddToken(TokenType.NUMBER, result);
    }

    private void ParseString()
    {
        while (Peek() != '"' && !AtEnd)
        {
            if (Peek() == '\n') line++;

            Advance();
        }

        if (AtEnd)
        {
            Lox.Error(line, "Unterminated string.");
            return;
        }

        // The closing ".
        Advance();

        // Trim the surrounding quotes.
        String value = source[(start + 1)..(current - 1)];
        AddToken(TokenType.STRING, value);
    }

    private char Peek()
    {
        if (AtEnd) return '\0';
        return source[current];
    }

    private char PeekNext()
    {
        if (current + 1 >= source.Length) return '\0';
        return source[current + 1];
    }

    private char Advance()
    {
        return source[current++];
    }

    private bool Match(char expected)
    {
        if (AtEnd || source[current] != expected) return false;

        current++;
        return true;
    }

    private void AddToken(TokenType type, object? literal = null)
    {
        var text = source[start..current];
        tokens.Add(new Token { Type = type,Lexeme = text,Literal = literal, Line = line });
    }
}
