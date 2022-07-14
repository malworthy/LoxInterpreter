using LoxInterpreter.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
public class Environment
{
    private readonly Dictionary<string, object?> values = new();
    private readonly Environment? enclosing = null;

    public Environment()
    {

    }

    public Environment(Environment enclosing)
    {
        this.enclosing = enclosing;
    }

    public void Define(string name, object? value)
    {
        values[name] = value;
    }

    public object? Get(Token name)
    {
        if (values.ContainsKey(name.Lexme))
            return values[name.Lexme];

        if (enclosing != null)
            return enclosing.Get(name);

        throw new RuntimeException(name, $"Variable {name.Lexme} not defined.");
    }

    public void Assign(Token name, object? value)
    {
        if (values.ContainsKey(name.Lexme))
        {
            values[name.Lexme] = value;
            return;
        }

        if (enclosing != null)
        {
            enclosing.Assign(name, value);
            return;
        }

        throw new RuntimeException(name, $"Variable '{name.Lexme}' not defined");
    }
}
