using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Expressions;

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
