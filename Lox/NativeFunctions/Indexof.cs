using LoxInterpreter.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.NativeFunctions;
internal class Indexof : ICallable
{
    public int Arity => 3;

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var str = arguments[0].ToString();
        var find = arguments[1].ToString();
        var startIndex = int.Parse(arguments[2].ToString());

        return (decimal)str.IndexOf(find,startIndex);
    }
}
