using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;
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

