namespace LoxInterpreter;

public class LoxClass : ICallable
{
    private readonly string name;
    private readonly Dictionary<string, LoxFunction> methods;

    public LoxClass? Superclass { get; init; }

    public LoxClass(string name, LoxClass superclass, Dictionary<string, LoxFunction> methods)
    {
        this.name = name;
        this.methods = methods;
        Superclass = superclass;
    }

    public int Arity => FindMethod("init")?.Arity ?? 0;

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var instance = new LoxInstance(this);
        var initializer = FindMethod("init");
        
        initializer?.Bind(instance)?.Call(interpreter, arguments);

        return instance;
    }

    public override string ToString() => name;

    internal LoxFunction? FindMethod(string name)
    {
        if(methods.ContainsKey(name))
            return methods[name];

        return Superclass?.FindMethod(name);
    }
}