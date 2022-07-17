using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
public interface ICallable
{
    int Arity { get; }
    object? Call(Interpreter interpreter, List<object?> arguments);
}
