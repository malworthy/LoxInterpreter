using LoxInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
public class Parser
{
    private readonly List<Token> tokens;
    private int current = 0;

    public bool AtEnd { get => Peek().Type == TokenType.EOF; }

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public Expr Parse()
    {
        try
        {
            return Expression();
        }
        catch (Exception ex)
        {
            return null;
            throw;
        }
    }

    private Expr Expression()
    {
        return Equality();
    }

    private Expr Equality()
    {
        var expr = ComparisonRule();

        while(Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            var oper = Previous();
            var right = ComparisonRule();
            expr = new Binary { Left = expr, Right = right, Operator = oper };
        }
        return expr;
    }

    private bool Match(params TokenType[] tokenTypes)
    {
        foreach(var type in tokenTypes)
        {
            if(Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private bool Check(TokenType type)
    {
        if (AtEnd) return false;
        return Peek().Type == type;
    }

    private Token Advance()
    {
        if (!AtEnd) current++;
        return Previous();
    }

    private Token Peek()
    {
        return tokens[current];
    }

    private Token Previous()
    {
        return tokens[current-1];
    }

    private Expr ComparisonRule()
    {
        var expr = Term();

        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token oper = Previous();
            Expr right = Term();
            expr = new Binary { Left = expr, Operator = oper, Right = right };
        }

        return expr;
    }

    private Expr Term()
    {
        Expr expr = Factor();

        while (Match(TokenType.MINUS, TokenType.PLUS))
        {
            Token oper = Previous();
            Expr right = Factor();
            expr = new Binary { Left= expr, Operator = oper, Right= right };
        }

        return expr;
    }

    private Expr Factor()
    {
        Expr expr = this.Unary();

        while (Match(TokenType.SLASH, TokenType.STAR))
        {
            Token oper = Previous();
            Expr right = this.Unary();
            expr = new Binary { Left = expr, Operator = oper, Right = right };
        }

        return expr;
    }

    private Expr Unary()
    {
        if(Match(TokenType.BANG, TokenType.MINUS))
        {
            var oper = Previous();
            var right = this.Unary();
            return new Unary { Operator = oper, Right = right };
        }
        return Primary();
    }

    private Expr Primary()
    {
        if (Match(TokenType.FALSE)) return new Literal { Value = false };
        if (Match(TokenType.TRUE)) return new Literal { Value = true };
        if (Match(TokenType.NIL)) return new Literal { Value = null };
        if (Match(TokenType.NUMBER, TokenType.STRING)) return new Literal { Value = Previous().Literal };
        if (Match(TokenType.LEFT_PAREN))
        {
            var expr = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Grouping { Expression = expr };
        }

        throw Error(Peek(), "Expression Expected");
    }

    private Token Consume(TokenType type, string messgge)
    {
        if (Check(type)) return Advance();

        throw Error(Peek(), messgge);
    }

    private Exception Error(Token token, string message)
    {
        Lox.Error(token, message);

        throw new ApplicationException(message);
    }

    private void Synchronize()
    {
        var statements = new TokenType[] 
        { 
            TokenType.CLASS, 
            TokenType.FUN,
            TokenType.VAR,
            TokenType.FOR,
            TokenType.IF,
            TokenType.WHILE,
            TokenType.PRINT,
            TokenType.RETURN
        };
        Advance();
        while(!AtEnd)
        {
            if (Previous().Type == TokenType.SEMICOLON) return;
            if (statements.Contains(Peek().Type)) return;
            Advance();
        }
    }
}