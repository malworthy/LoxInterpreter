using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Expressions;

public abstract class Expr
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}









