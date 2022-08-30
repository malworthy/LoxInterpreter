using LoxInterpreter.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.NativeFunctions;
internal class Substr : ICallable
{
    public int Arity => 3;

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var str = arguments[0].ToString();
        var start = int.Parse(arguments[1].ToString());
        var length = int.Parse(arguments[2].ToString());

        return str.Substring(start, length);
    }
}
