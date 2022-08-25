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
    T Visit(Super expr);
    T Visit(Get expr);
    T Visit(Variable expr);
    T Visit(Logical expr);
    T Visit(Call expr);
    T Visit(Set expr);
}
