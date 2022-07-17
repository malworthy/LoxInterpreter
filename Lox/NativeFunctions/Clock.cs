using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.NativeFunctions;
public class Clock : ICallable
{
    public int Arity { get; init; }

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        return (decimal)DateTime.Now.ToFileTime();
    }
}
