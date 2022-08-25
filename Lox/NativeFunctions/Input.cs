using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.NativeFunctions;
internal class Input : ICallable
{
    public int Arity => 0;

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        return Console.ReadLine();
    }
}
