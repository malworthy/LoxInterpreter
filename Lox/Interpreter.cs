using LoxInterpreter.Exceptions;
using LoxInterpreter.Expressions;
using LoxInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
public class Interpreter : Expressions.IVisitor<object?>, Statements.IVisitor<bool>
{
    private Environment environment = new();
    private readonly IOutput output;

    public Interpreter(IOutput output)
    {
        this.output = output;
    }
    public string Interpret(Expr expr)
    {
        try
        {
            var value = Evaluate(expr);
            return value?.ToString() ?? "Nil";
        }
        catch (RuntimeException ex)
        {
            Lox.Error(ex.Token, ex.Message);
            return "Error";
        }
       
    }

    public void Interpret(List<Stmt> statements)
    {
        try
        {
            foreach (var statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeException error)
        {
            Lox.RuntimeError(error);
        }
    }

    private void Execute(Stmt stmt)
    {
        stmt.Accept(this);
    }

    public object? Visit(Assign expr)
    {
        var value = Evaluate(expr.Value);
        environment.Assign(expr.Name, value);
        return value;
    }

    public object? Visit(Binary expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        switch (expr.Operator.Type)
        {
            case TokenType.BANG_EQUAL:
                return !IsEqual(left, right);
            case TokenType.EQUAL_EQUAL:
                return IsEqual(left,right);
        }

        if (left is string && right is string)
        {
            var leftStr = (string?)left;
            var rightStr = (string?)right;
            switch (expr.Operator.Type)
            {
                case TokenType.PLUS:
                    return leftStr + rightStr;
                case TokenType.GREATER:
                    return string.Compare(leftStr, rightStr) > 0;
                case TokenType.GREATER_EQUAL:
                    return string.Compare(leftStr, rightStr) >= 0;
                case TokenType.LESS:
                    return string.Compare(leftStr, rightStr) < 0;
                case TokenType.LESS_EQUAL:
                    return string.Compare(leftStr, rightStr) <= 0;
            }
        }

        if(left is decimal && right is decimal)
        {
            return EvaluateNumbers((decimal)left, (decimal)right, expr.Operator);
        }

        throw new RuntimeException(expr.Operator, "Type mismatch");

    }

    private object? EvaluateNumbers(decimal leftNumber, decimal rightNumber, Token oper)
    {
        if (rightNumber == 0 && oper.Type == TokenType.SLASH)
            throw new RuntimeException(oper, "Divide by zero error");
        
        try
        {
            switch (oper.Type)
            {
                case TokenType.MINUS:
                    return leftNumber - rightNumber;
                case TokenType.PLUS:
                    return leftNumber + rightNumber;
                case TokenType.STAR:
                    return leftNumber * rightNumber;
                case TokenType.SLASH:
                    return leftNumber / rightNumber;
                case TokenType.GREATER:
                    return leftNumber > rightNumber;
                case TokenType.GREATER_EQUAL:
                    return leftNumber >= rightNumber;
                case TokenType.LESS:
                    return leftNumber < rightNumber;
                case TokenType.LESS_EQUAL:
                    return leftNumber <= rightNumber;
            }
        }
        catch (Exception ex)
        {
            throw new RuntimeException(oper, ex.Message);
        }
        return null;
    }

    private bool IsEqual(object? left, object? right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;

        return left.Equals(right);
    }

    public object? Visit(Grouping expr)
    {
        return Evaluate(expr.Expression);
    }

    public object? Visit(Literal expr)
    {
        return expr.Value;
    }

    public object? Visit(Unary expr)
    {
        var right = Evaluate(expr.Right);
        if (expr.Operator.Type == TokenType.MINUS)
            return -(decimal)right!;
        else if (expr.Operator.Type == TokenType.BANG)
            return !IsTruthy(right);

        return null;
    }

    private bool IsTruthy(object? right)
    {
        if (right == null) return false;

        return (right as bool?) ?? true;
    }

    private object? Evaluate(Expr expr)
    {
        return expr.Accept(this);
    }

    public bool Visit(Expression expr)
    {
        Evaluate(expr.expression);
        return true;
    }

    public bool Visit(Print expr)
    {
        var value = Evaluate(expr.expression);
        output.Print(value);
        return true;
    }

    public bool Visit(Var stmt)
    {
        object? value = null;
        if (stmt.Initializer != null)
            value = Evaluate(stmt.Initializer);

        environment.Define(stmt.Name.Lexme, value);
        return true;
    }

    public object? Visit(Variable expr)
    {
        return environment.Get(expr.Name);
    }

    public bool Visit(Block stmt)
    {
        ExecuteBlock(stmt.Statements, new Environment(environment));
        return true;
    }

    private void ExecuteBlock(IEnumerable<Stmt> statements, Environment environment)
    {
        var previous = this.environment;
        try
        {
            this.environment = environment;
            statements.ToList().ForEach(x => Execute(x));
        }
        finally
        {
            this.environment = previous;
        }
    }

    public bool Visit(If stmt)
    {
        if (IsTruthy(Evaluate(stmt.Condition)))
            Execute(stmt.ThenBranch);
        else if (stmt.ElseBranch != null)
            Execute(stmt.ElseBranch);

        return true;
    }

    public object? Visit(Logical expr)
    {
        var left = Evaluate(expr.Left);

        if (expr.Operator.Type == TokenType.OR)
        {
            if (IsTruthy(left)) return left;
        }
        else
        {
            if (!IsTruthy(left)) return left;
        }

        return Evaluate(expr.Right);
    }

    public bool Visit(While stmt)
    {
        while (IsTruthy(Evaluate(stmt.Condition)))
            Execute(stmt.Body);
        return true;
    }
}
