using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;

public abstract class Stmt
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}

