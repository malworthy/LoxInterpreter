﻿using LoxInterpreter.Exceptions;
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
    public Environment? Enclosing => enclosing;

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
        if (values.ContainsKey(name.Lexeme))
            return values[name.Lexeme];

        if (enclosing != null)
            return enclosing.Get(name);

        throw new RuntimeException(name, $"Variable {name.Lexeme} not defined.");
    }

    public void Assign(Token name, object? value)
    {
        if (values.ContainsKey(name.Lexeme))
        {
            values[name.Lexeme] = value;
            return;
        }

        if (enclosing != null)
        {
            enclosing.Assign(name, value);
            return;
        }

        throw new RuntimeException(name, $"Variable '{name.Lexeme}' not defined");
    }

    internal object? GetAt(int distance, string name)
    {
        var ans = Ancestor(distance);

        return ans.values[name];
    }

    private Environment Ancestor(int distance)
    {
        var env = this;

        for(int i = 0; i < distance; i++)
        {
            env = env.enclosing;
        }

        return env;
    }

    internal void AssignAt(int distance, Token name, object? value)
    {
        Ancestor(distance).values[name.Lexeme] = value;
    }
}
