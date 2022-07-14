using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;
public class Block : Stmt
{
    public IEnumerable<Stmt> Statements { get; init; }

    public Block(IEnumerable<Stmt> statements)
    {
        Statements = statements;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
