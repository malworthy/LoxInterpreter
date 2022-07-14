using LoxInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;
public class If : Stmt
{
    public Expr Condition { get; init; }
    public Stmt ThenBranch { get; init; }
    public Stmt? ElseBranch { get; init; }

    public If(Expr condition, Stmt thenBranch, Stmt? elseBranch)
    {
        Condition = condition;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        
        return visitor.Visit(this);
    }
}
