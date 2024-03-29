﻿using LoxInterpreter.Exceptions;
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
    private readonly Environment closure;
    private readonly bool isInitializer;

    public int Arity { get => declaration.Parameters.Count(); }

    public LoxFunction(Function declaration, Environment closure, bool isInitializer)
    {
        this.declaration = declaration;
        this.closure = closure;
        this.isInitializer = isInitializer;
    }
    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var environment = new Environment(closure);
        var i = 0;
        foreach(var param in declaration.Parameters)
        {
            environment.Define(param.Lexeme, arguments[i++]);
        }
        try
        {
            interpreter.ExecuteBlock(declaration.Body, environment);
        }
        catch (ReturnException returnValue)
        {
            return returnValue.Value;
        }
        if (isInitializer) return closure.GetAt(0, "this");

        return null;
    }

    internal LoxFunction Bind(LoxInstance loxInstance)
    {
        var env = new Environment(closure);

        env.Define("this", loxInstance);

        return new LoxFunction(declaration, env, isInitializer);
    }
}
