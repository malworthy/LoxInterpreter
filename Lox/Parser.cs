using LoxInterpreter.Exceptions;
using LoxInterpreter.Expressions;
using LoxInterpreter.Statements;
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

    public Expr ParseExpression()
    {
        try
        {
            return Expression();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public List<Stmt> Parse()
    {
        var statements = new List<Stmt>();
        while (!AtEnd)
        {
            var stmt = Declaration();
            if (stmt != null)
                statements.Add(stmt);
        }

        return statements;
    }

    private Stmt? Declaration()
    {
        try
        {
            if (Match(TokenType.CLASS)) 
                return ClassDeclaration();
            if (Match(TokenType.FUN))
                return Function("function");
            if (Match(TokenType.VAR))
                return VarDeclaration();

            return Statement();
        }
        catch (ParseException)
        {
            Synchronize();
            return null;
        }
    }

    private Stmt? ClassDeclaration()
    {
        var name = Consume(TokenType.IDENTIFIER, "Expect class name");

        Variable? superclass = null;

        if (Match(TokenType.LESS))
        {
            Consume(TokenType.IDENTIFIER, "Expect superclass name");
            superclass = new Variable(Previous());
        }


        Consume(TokenType.LEFT_BRACE, "Missing {");

        var methods = new List<Function>();

        while (!Check(TokenType.RIGHT_BRACE) && !AtEnd)
            methods.Add(Function("method"));

        Consume(TokenType.RIGHT_BRACE, "Missing }");

        return new Statements.Class(name, superclass, methods);

    }

    private Function Function(string kind)
    {
        var name = Consume(TokenType.IDENTIFIER, $"Expect {kind} name.");

        Consume(TokenType.LEFT_PAREN, $"Missing '(' after {kind}.");
        
        var parameters = new List<Token>();
        
        if (!Check(TokenType.RIGHT_PAREN))
        {
            do
            {
                if (parameters.Count > 255)
                    Error(Peek(), "A cannot have more than 255 parameters");
                parameters.Add(Consume(TokenType.IDENTIFIER,"Expect parameter name"));
            } while (Match(TokenType.COMMA));
        }
        
        Consume(TokenType.RIGHT_PAREN, "Missing ')'");
        Consume(TokenType.LEFT_BRACE, $"Missing '{{' at start of {kind} body. ");
        
        var body = GetBlock();

        return new Statements.Function(name, parameters, body);
    }

    private Stmt VarDeclaration()
    {
        var name = Consume(TokenType.IDENTIFIER, "Expect variable name.");
        Expr? initializer = null;

        if (Match(TokenType.EQUAL))
        {
            initializer = Expression();
        }

        Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
        return new Statements.Var(name, initializer);
    }

    private Stmt Statement()
    {
        if (Match(TokenType.FOR)) return ForStatement();
        if (Match(TokenType.IF)) return IfStatement();
        if (Match(TokenType.PRINT)) return PrintStatement();
        if (Match(TokenType.RETURN)) return ReturnStatement();
        if (Match(TokenType.WHILE)) return WhileStatement();
        if (Match(TokenType.LEFT_BRACE)) return new Block(GetBlock());

        return ExpressionStatement();
    }

    private Stmt ReturnStatement()
    {
        var keyword = Previous();
        var value = !Check(TokenType.SEMICOLON) ? Expression() : null;

        Consume(TokenType.SEMICOLON, "Expect ';' after return value.");
        return new Statements.Return(keyword, value);
    }

    private Stmt ForStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Expect '(' after 'for'.");

        Stmt? initializer;
        if (Match(TokenType.SEMICOLON))
            initializer = null;
        else if (Match(TokenType.VAR))
            initializer = VarDeclaration();
        else
            initializer = ExpressionStatement();

        Expr? condition = null;
        if (!Check(TokenType.SEMICOLON))
            condition = Expression();
        Consume(TokenType.SEMICOLON, "; missing");

        Expr? increment = null;
        if (!Check(TokenType.RIGHT_PAREN))
            increment = Expression();
        Consume(TokenType.RIGHT_PAREN, "Expect ')' after for clauses.");

        var body = Statement();

        if (increment != null)
        {
            body = new Block(new List<Stmt> { body, new Expression(increment) });
        }

        if (condition == null) condition = new Literal(true);
        body = new While(condition, body);

        if (initializer != null)
        {
            body = new Block(new List<Stmt> { initializer, body });
        }

        return body;

    }

    private Stmt WhileStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Missing '(' after 'while'");
        var condition = Expression();
        Consume(TokenType.RIGHT_PAREN, "Missing ')' after condition");
        var body = Statement();
        return new Statements.While(condition, body);
    }

    private Stmt IfStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
        var condition = Expression();
        Consume(TokenType.RIGHT_PAREN, "Expect ')' after 'if' statement.");
        
        var thenBranch = Statement();
        var elseBranch = Match(TokenType.ELSE) ? Statement() : null;

        return new Statements.If(condition, thenBranch, elseBranch);
    }

    private IEnumerable<Stmt> GetBlock()
    {
        var statements = new List<Stmt>();
        while(!Check(TokenType.RIGHT_BRACE) && !AtEnd)
        {
            var statement = Declaration();
            if (statement!=null)
                statements.Add(statement);
        }

        Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
        return statements;
    }
    private Stmt PrintStatement()
    {
        Expr value = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after value.");
        return new Print(value);
    }

    private Stmt ExpressionStatement()
    {
        Expr value = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
        return new Statements.Expression(value);
    }

    private Expr Expression()
    {
        return Assignment();
    }

    private Expr Assignment()
    {
        var expr = Or();

        if (Match(TokenType.PLUS_PLUS))
        {
            var variable = expr as Variable;

            if (variable == null)
            {
                Error(Peek(), "Can only apply ++ to a variable.");
                return expr;
            }

            var plus = new Token 
            { 
                Lexeme = "+", 
                Line = variable?.Name.Line ?? 0, 
                Type = TokenType.PLUS
            };

            var value = new Binary
            {
                Left = expr,
                Operator = plus,
                Right = new Literal(1M)
            };

            return new Assign(variable!.Name, value);
        }

        if (Match(TokenType.EQUAL))
        {
            Token equals = Previous();
            Expr value = Assignment();

            if (expr is Variable variable) 
            {
                Token name = variable.Name;
                return new Assign(name, value);
            } 
            else if (expr is Get get)
            {
                return new Expressions.Set(get.Object, get.Name, value);
            }

            Error(equals, "Invalid assignment target.");
        }

        return expr;
    }

    private Expr Or()
    {
        var expr = And();

        while(Match(TokenType.OR))
        {
            var oper = Previous();
            var right = And();
            expr = new Logical(expr, oper, right);
        }

        return expr;
    }

    private Expr And()
    {
        var expr = Equality();

        while (Match(TokenType.OR))
        {
            var oper = Previous();
            var right = Equality();
            expr = new Logical(expr, oper, right);
        }

        return expr;
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
        return Call();
    }

    private Expr Call()
    {
        var expr = Primary();
        while (true)
        {
            if (Match(TokenType.LEFT_PAREN))
            {
                expr = FinishCall(expr);
            }
            else if (Match(TokenType.DOT))
            {
                var name = Consume(TokenType.IDENTIFIER, "Property name missing after '.'");
                expr = new Expressions.Get(expr, name);
            }
            else
            {
                break;
            }
                
        }

        return expr;
    }

    private Expr FinishCall(Expr callee)
    {
        var arguments = new List<Expr>();
        if (!Check(TokenType.RIGHT_PAREN))
        {
            do
            {
                if (arguments.Count > 255)
                    Error(Peek(), "A function cannot have more than 255 arguments");
                arguments.Add(Expression());
            } while (Match(TokenType.COMMA));
        }
        var paran = Consume(TokenType.RIGHT_PAREN, "Missing ')'");
        return new Expressions.Call(callee, paran, arguments);
    }

    private Expr Primary()
    {
        if (Match(TokenType.FALSE)) return new Literal(false);
        if (Match(TokenType.TRUE)) return new Literal(true);
        if (Match(TokenType.NIL)) return new Literal(null);
        if (Match(TokenType.NUMBER, TokenType.STRING)) return new Literal(Previous().Literal);
        if (Match(TokenType.THIS)) return new Expressions.This(Previous());

        if (Match(TokenType.SUPER))
        {
            var keyword = Previous();
            Consume(TokenType.DOT, "Expect '.' after super");
            var method = Consume(TokenType.IDENTIFIER, "Expect superclass method name");
            return new Expressions.Super(keyword, method);
        }

        if (Match(TokenType.IDENTIFIER)) return new Expressions.Variable(Previous());

        if (Match(TokenType.LEFT_PAREN))
        {
            var expr = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Grouping { Expression = expr };
        }

        throw Error(Peek(), "Expression Expected");
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) return Advance();

        throw Error(Peek(), message);
    }

    private Exception Error(Token token, string message)
    {
        Lox.Error(token, message);

        throw new ParseException(message);
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