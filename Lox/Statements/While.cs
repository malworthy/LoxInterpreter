using LoxInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;
public class While : Stmt
{
    public Expr Condition { get; init; }
    public Stmt Body { get; init; }

    public While(Expr condition, Stmt body)
    {
        Condition = condition;
        Body = body;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
