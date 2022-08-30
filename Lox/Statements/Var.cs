using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;

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
