using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;

public interface IVisitor<T>
{
    T Visit(Expression expr);
    T Visit(Print expr);
    T Visit(Var variable);
}
public abstract class Stmt
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}

public class Expression : Stmt
{
    public Expressions.Expr expression { get; init; }

    public Expression(Expressions.Expr expression)
    {
        this.expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Print : Stmt
{
    public Expressions.Expr expression { get; init; }

    public Print(Expressions.Expr expression)
    {
        this.expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Var : Stmt
{
    public Token Name { get; init; }
    public Expressions.Expr? Initializer { get; init; }

    public Var(Token name, Expressions.Expr? initializer)
    {
        Name = name;
        Initializer = initializer;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}