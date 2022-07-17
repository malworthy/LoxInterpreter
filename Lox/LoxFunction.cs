using LoxInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
public class LoxFunction : ICallable
{
    private readonly Function declaration;
    public int Arity { get => declaration.Parameters.Count(); }

    public LoxFunction(Function declaration)
    {
        this.declaration = declaration;
    }
    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var environment = new Environment(interpreter.globals);
        var i = 0;
        foreach(var param in declaration.Parameters)
        {
            environment.Define(param.Lexme, arguments[i++]);
        }
        interpreter.ExecuteBlock(declaration.Body, environment);
        return null;
    }
}
