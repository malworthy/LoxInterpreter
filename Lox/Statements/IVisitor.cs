using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Statements;
public interface IVisitor<T>
{
    T Visit(Expression stmt);
    T Visit(Print stmt);
    T Visit(Var stmt);
    T Visit(Block stmt);
    T Visit(If stmt);
    T Visit(While stmt);
}
