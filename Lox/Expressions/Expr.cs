using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Expressions;

public interface IVisitor<T>
{
    T Visit(Assign expr);
    T Visit(Binary expr);
    T Visit(Grouping expr);
    T Visit(This expr);
    T Visit(Literal expr);
    T Visit(Unary expr);
    T Visit(Get expr);
    T Visit(Variable expr);
    T Visit(Logical expr);
    T Visit(Call expr);
    T Visit(Set expr);
}

public abstract class Expr
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}

public class Assign : Expr
{
    public Token Name { get; init; }
    public Expr Value { get; init; }

    public Assign(Token name, Expr value)
    {
        Name = name;
        Value = value;  
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Binary : Expr
{
    public Expr Left { get; init; }
    public Token Operator { get; init; }
    public Expr Right { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Grouping : Expr
{
    public Expr Expression { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Literal : Expr
{
    public object? Value { get; init; }

    public Literal(object? value)
    {
        Value = value;  
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Unary : Expr
{
    public  Token Operator { get; init; }
    public Expr Right { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Variable : Expr
{
    public Token Name { get; init; }

    public Variable(Token name) 
    {
        Name = name;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    
}